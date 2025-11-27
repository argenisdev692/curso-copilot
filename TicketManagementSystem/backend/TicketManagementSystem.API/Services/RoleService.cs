using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Service for role business logic operations
/// </summary>
public class RoleService : BaseService, IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<RoleService> logger) : base(logger)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }

    /// <inheritdoc />
    public async Task<RoleDto> GetRoleByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found");
        }

        return _mapper.Map<RoleDto>(role);
    }

    /// <inheritdoc />
    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto)
    {
        // Check if role name already exists - business rule validation
        var existingRole = await _roleRepository.GetByNameAsync(roleDto.RoleName);
        if (existingRole != null)
        {
            throw new InvalidOperationException("Role with this name already exists");
        }

        var role = _mapper.Map<Role>(roleDto);
        role.CreatedAt = DateTime.UtcNow;
        role.UpdatedAt = DateTime.UtcNow;

        await _roleRepository.AddAsync(role);

        LogInformation("Role {RoleId} created", role.Id);

        return _mapper.Map<RoleDto>(role);
    }

    /// <inheritdoc />
    public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto roleDto)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found");
        }

        // Check if role name already exists for another role
        if (!string.IsNullOrEmpty(roleDto.RoleName) &&
            await _roleRepository.RoleNameExistsAsync(roleDto.RoleName, id))
        {
            throw new InvalidOperationException("Role with this name already exists");
        }

        _mapper.Map(roleDto, role);
        role.UpdatedAt = DateTime.UtcNow;

        await _roleRepository.UpdateAsync(role);

        LogInformation("Role {RoleId} updated", id);

        return _mapper.Map<RoleDto>(role);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            return false;
        }

        // Soft delete - business logic
        role.IsDeleted = true;
        role.UpdatedAt = DateTime.UtcNow;
        await _roleRepository.UpdateAsync(role);

        LogInformation("Role {RoleId} soft deleted", id);

        return true;
    }
}