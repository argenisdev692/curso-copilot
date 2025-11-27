using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for user business logic operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get paginated users with optional filters and sorting
        /// </summary>
        /// <param name="parameters">Query parameters for pagination, filters, and sorting</param>
        /// <returns>Paged response containing users</returns>
        Task<PagedResponse<UserDto>> GetUsersAsync(GetUsersQueryParameters parameters);

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User DTO</returns>
        Task<UserDto> GetUserByIdAsync(int id);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user DTO</returns>
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userDto">User update data</param>
        /// <returns>Updated user DTO</returns>
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>True if deleted successfully</returns>
        Task<bool> DeleteUserAsync(int id);
    }
}