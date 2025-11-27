using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories;

/// <summary>
/// Repository interface for role data operations
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Get a role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role or null</returns>
    Task<Role?> GetByIdAsync(int id);

    /// <summary>
    /// Get a role by name
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <returns>Role or null</returns>
    Task<Role?> GetByNameAsync(string roleName);

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>List of roles</returns>
    Task<List<Role>> GetAllAsync();

    /// <summary>
    /// Add a new role
    /// </summary>
    /// <param name="role">Role to add</param>
    Task AddAsync(Role role);

    /// <summary>
    /// Update an existing role
    /// </summary>
    /// <param name="role">Role to update</param>
    Task UpdateAsync(Role role);

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="role">Role to delete</param>
    Task DeleteAsync(Role role);

    /// <summary>
    /// Check if a role exists
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>True if role exists</returns>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Check if a role name exists
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <param name="excludeId">Exclude this ID from check</param>
    /// <returns>True if role name exists</returns>
    Task<bool> RoleNameExistsAsync(string roleName, int? excludeId = null);
}