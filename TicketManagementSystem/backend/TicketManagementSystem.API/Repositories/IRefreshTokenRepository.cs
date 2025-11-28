using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Repository interface for refresh token data operations
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Get a refresh token by its hash
        /// </summary>
        /// <param name="tokenHash">Hashed token value</param>
        /// <returns>Refresh token or null</returns>
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

        /// <summary>
        /// Get all active refresh tokens for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of active refresh tokens</returns>
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);

        /// <summary>
        /// Add a new refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token to add</param>
        Task AddAsync(RefreshToken refreshToken);

        /// <summary>
        /// Update an existing refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token to update</param>
        Task UpdateAsync(RefreshToken refreshToken);

        /// <summary>
        /// Revoke a refresh token
        /// </summary>
        /// <param name="tokenHash">Token hash to revoke</param>
        /// <param name="reason">Reason for revocation</param>
        Task RevokeTokenAsync(string tokenHash, string? reason = null);

        /// <summary>
        /// Revoke all refresh tokens for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="reason">Reason for revocation</param>
        Task RevokeAllTokensForUserAsync(int userId, string? reason = null);

        /// <summary>
        /// Clean up expired tokens
        /// </summary>
        /// <returns>Number of tokens cleaned up</returns>
        Task<int> CleanupExpiredTokensAsync();

        /// <summary>
        /// Check if a token exists and is active
        /// </summary>
        /// <param name="tokenHash">Token hash</param>
        /// <returns>True if token is active</returns>
        Task<bool> IsTokenActiveAsync(string tokenHash);
    }
}