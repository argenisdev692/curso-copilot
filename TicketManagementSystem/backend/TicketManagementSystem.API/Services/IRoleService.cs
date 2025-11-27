using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Interface for role business logic operations
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>List of role DTOs</returns>
    Task<List<RoleDto>> GetRolesAsync();

    /// <summary>
    /// Get a role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role DTO</returns>
    Task<RoleDto> GetRoleByIdAsync(int id);

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="roleDto">Role creation data</param>
    /// <returns>Created role DTO</returns>
    Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto);

    /// <summary>
    /// Update an existing role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="roleDto">Role update data</param>
    /// <returns>Updated role DTO</returns>
    Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto roleDto);

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id">Role ID to delete</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> DeleteRoleAsync(int id);
}