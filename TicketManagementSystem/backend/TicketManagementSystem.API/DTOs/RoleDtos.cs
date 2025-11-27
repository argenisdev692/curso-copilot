using System;

namespace TicketManagementSystem.API.DTOs;

/// <summary>
/// Data Transfer Object for Role entity
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Unique identifier of the role
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the role
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Description of the role
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Creation timestamp in UTC
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp in UTC
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO for creating a new role
/// </summary>
public class CreateRoleDto
{
    /// <summary>
    /// Name of the role (required, unique)
    /// </summary>
    public required string RoleName { get; set; }

    /// <summary>
    /// Description of the role
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// DTO for updating a role
/// </summary>
public class UpdateRoleDto
{
    /// <summary>
    /// Name of the role
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Description of the role
    /// </summary>
    public string? Description { get; set; }
}