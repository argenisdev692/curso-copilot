using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.MediatR.Commands;
using TicketManagementSystem.API.MediatR.Queries;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing users with CQRS pattern
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createValidator;
        private readonly IValidator<UpdateUserDto> _updateValidator;
        private readonly ICacheHelper _cacheHelper;

        /// <summary>
        /// Constructor for UsersController
        /// </summary>
        public UsersController(
            IMediator mediator,
            ILogger<UsersController> logger,
            IUserService userService,
            IValidator<CreateUserDto> createValidator,
            IValidator<UpdateUserDto> updateValidator,
            ICacheHelper cacheHelper)
            : base(logger)
        {
            _mediator = mediator;
            _userService = userService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _cacheHelper = cacheHelper;
        }

        /// <summary>
        /// Get paginated list of users with optional filters and sorting
        /// </summary>
        /// <param name="parameters">Query parameters for pagination, filters, and sorting</param>
        /// <returns>Paged response containing users</returns>
        /// <response code="200">Returns paginated list of users</response>
        /// <response code="400">Invalid query parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<UserDto>>> GetUsers(
            [FromQuery] GetUsersQueryParameters parameters)
        {
            try
            {
                var query = new GetUsersQuery { Parameters = parameters };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="400">Invalid user ID</response>
        /// <response code="404">User not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var cacheKey = $"user_{id}";
                var user = await _cacheHelper.GetOrSetAsync(
                    cacheKey,
                    () => _userService.GetUserByIdAsync(id),
                    TimeSpan.FromMinutes(10));

                _logger.LogInformation("Retrieved user {UserId}", id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("User {UserId} not found", id);
                return NotFound(CreateProblemDetails("Not Found", "User not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "GetUser", id);
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid user data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="409">User already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            var validationResult = await _createValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return HandleValidationErrors(errors, "CreateUser");
            }

            try
            {
                var createdUser = await _userService.CreateUserAsync(userDto);
                await InvalidateUsersCache();

                _logger.LogInformation("Created new user {UserId} with email {Email}", createdUser.Id, createdUser.Email);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                _logger.LogWarning("Attempted to create user with existing email {Email}", userDto.Email);
                return Conflict(CreateProblemDetails("Conflict", "User with this email already exists", StatusCodes.Status409Conflict));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "CreateUser", userDto.Email);
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userDto">User update data</param>
        /// <returns>Updated user</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid user data</response>
        /// <response code="404">User not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return HandleValidationErrors(errors, $"UpdateUser for user {id}");
            }

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);

                await InvalidateUsersCache();
                await _cacheHelper.RemoveAsync($"user_{id}");

                _logger.LogInformation("Updated user {UserId}", id);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("User {UserId} not found for update", id);
                return NotFound(CreateProblemDetails("Not Found", "User not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "UpdateUser", id);
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">User deleted successfully</response>
        /// <response code="400">Invalid user ID</response>
        /// <response code="404">User not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("User {UserId} not found for deletion", id);
                    return NotFound(CreateProblemDetails("Not Found", "User not found", StatusCodes.Status404NotFound));
                }

                await InvalidateUsersCache();
                await _cacheHelper.RemoveAsync($"user_{id}");

                _logger.LogInformation("Deleted user {UserId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, "DeleteUser", id);
            }
        }

        /// <summary>
        /// Invalidate all users-related cache entries
        /// </summary>
        private async Task InvalidateUsersCache()
        {
            try
            {
                await _cacheHelper.InvalidatePatternAsync("users_*");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error invalidating users cache");
            }
        }
    }
}