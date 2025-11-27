using System;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Data Transfer Object for User entity
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unique identifier of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Role of the user (Admin, Agent, User)
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Whether the user account is active
        /// </summary>
        public bool IsActive { get; set; }

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
    /// DTO for creating a new user
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Email address (required, unique)
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Password (required, will be hashed)
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Full name (required)
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// Role (Admin, Agent, User) - defaults to User
        /// </summary>
        public string Role { get; set; } = "User";
    }

    /// <summary>
    /// DTO for updating a user
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Role (Admin, Agent, User)
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Whether the user account is active
        /// </summary>
        public bool? IsActive { get; set; }
    }
}