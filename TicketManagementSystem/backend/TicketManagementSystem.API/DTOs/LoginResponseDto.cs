namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for login response
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT access token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// User information
        /// </summary>
        public UserBasicDto User { get; set; } = null!;
    }
}