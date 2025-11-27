namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for refresh token response
    /// </summary>
    public class RefreshTokenResponseDto
    {
        /// <summary>
        /// New JWT access token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// New refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}