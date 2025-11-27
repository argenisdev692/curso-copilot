using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for authentication business logic operations
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user and generate JWT token
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Login response with token</returns>
        Task<LoginResponseDto> LoginAsync(string email, string password);

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="dto">Registration data</param>
        /// <returns>Registration response</returns>
        Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>New token response</returns>
        Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Logout user and invalidate refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token to invalidate</param>
        /// <returns>Task</returns>
        Task LogoutAsync(string refreshToken);
    }
}