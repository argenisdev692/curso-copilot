namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for registration response
    /// </summary>
    public class RegisterResponseDto
    {
        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Created user information
        /// </summary>
        public UserBasicDto User { get; set; } = null!;
    }
}