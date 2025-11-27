using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;
using TicketManagementSystem.API.Validators;

namespace TicketManagementSystem.API.Controllers;

/// <summary>
/// Controller for managing roles
/// </summary>
[Route("api/[controller]")]
[Authorize(Policy = "RequireAuthenticatedUser")]
public class RolesController : BaseApiController
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRoleDto> _createValidator;
    private readonly IValidator<UpdateRoleDto> _updateValidator;
    private readonly CacheHelper _cacheHelper;

    /// <summary>
    /// Constructor for RolesController
    /// </summary>
    public RolesController(
        IRoleService roleService,
        IMapper mapper,
        ILogger<RolesController> logger,
        IValidator<CreateRoleDto> createValidator,
        IValidator<UpdateRoleDto> updateValidator,
        CacheHelper cacheHelper)
        : base(logger)
    {
        _roleService = roleService;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _cacheHelper = cacheHelper;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>List of roles</returns>
    /// <response code="200">Returns list of roles</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        try
        {
            var cacheKey = "roles";
            var roles = await _cacheHelper.GetOrSetAsync(
                cacheKey,
                () => _roleService.GetRolesAsync(),
                TimeSpan.FromMinutes(10));

            _logger.LogInformation("Retrieved {Count} roles", roles?.Count ?? 0);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "GetRoles");
        }
    }

    /// <summary>
    /// Get a specific role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role details</returns>
    /// <response code="200">Returns the role</response>
    /// <response code="400">Invalid role ID</response>
    /// <response code="404">Role not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> GetRole(int id)
    {
        try
        {
            var cacheKey = $"role_{id}";
            var role = await _cacheHelper.GetOrSetAsync(
                cacheKey,
                () => _roleService.GetRoleByIdAsync(id),
                TimeSpan.FromMinutes(10));

            _logger.LogInformation("Retrieved role {RoleId}", id);
            return Ok(role!);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Role {RoleId} not found", id);
            return NotFound(CreateProblemDetails("Not Found", "Role not found", StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            return HandleException(ex, "GetRole", id);
        }
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="roleDto">Role creation data</param>
    /// <returns>Created role</returns>
    /// <response code="201">Role created successfully</response>
    /// <response code="400">Invalid role data</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Role already exists</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto roleDto)
    {
        var validationResult = await _createValidator.ValidateAsync(roleDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return HandleValidationErrors(errors, "CreateRole");
        }

        try
        {
            var createdRole = await _roleService.CreateRoleAsync(roleDto);
            await InvalidateRolesCache();

            _logger.LogInformation("Created new role {RoleId} with name {RoleName}", createdRole.Id, createdRole.RoleName);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogWarning("Attempted to create role with existing name {RoleName}", roleDto.RoleName);
            return Conflict(CreateProblemDetails("Conflict", "Role with this name already exists", StatusCodes.Status409Conflict));
        }
        catch (Exception ex)
        {
            return HandleException(ex, "CreateRole", roleDto.RoleName);
        }
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="roleDto">Role update data</param>
    /// <returns>Updated role</returns>
    /// <response code="200">Role updated successfully</response>
    /// <response code="400">Invalid role data</response>
    /// <response code="404">Role not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Role name already exists</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> UpdateRole(int id, [FromBody] UpdateRoleDto roleDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(roleDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return HandleValidationErrors(errors, "UpdateRole");
        }

        try
        {
            var updatedRole = await _roleService.UpdateRoleAsync(id, roleDto);
            await InvalidateRolesCache();

            _logger.LogInformation("Updated role {RoleId}", id);
            return Ok(updatedRole);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Role {RoleId} not found for update", id);
            return NotFound(CreateProblemDetails("Not Found", "Role not found", StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogWarning("Attempted to update role {RoleId} with existing name", id);
            return Conflict(CreateProblemDetails("Conflict", "Role with this name already exists", StatusCodes.Status409Conflict));
        }
        catch (Exception ex)
        {
            return HandleException(ex, "UpdateRole", id);
        }
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id">Role ID to delete</param>
    /// <returns>Success status</returns>
    /// <response code="204">Role deleted successfully</response>
    /// <response code="404">Role not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Role {RoleId} not found for deletion", id);
                return NotFound(CreateProblemDetails("Not Found", "Role not found", StatusCodes.Status404NotFound));
            }

            await InvalidateRolesCache();

            _logger.LogInformation("Deleted role {RoleId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "DeleteRole", id);
        }
    }

    private async Task InvalidateRolesCache()
    {
        await _cacheHelper.RemoveAsync("roles");
        // Note: Individual role caches will be invalidated by cache expiration
    }
}