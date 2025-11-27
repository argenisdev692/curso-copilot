using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Repository interface for user data operations
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User or null</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User or null</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Get paginated users
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Paged response of users</returns>
        Task<PagedResponse<User>> GetUsersAsync(GetUsersQueryParameters parameters);

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User to add</param>
        Task AddAsync(User user);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user">User to update</param>
        Task UpdateAsync(User user);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User to delete</param>
        Task DeleteAsync(User user);

        /// <summary>
        /// Check if a user exists
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>True if user exists</returns>
        Task<bool> ExistsAsync(int id);
    }
}