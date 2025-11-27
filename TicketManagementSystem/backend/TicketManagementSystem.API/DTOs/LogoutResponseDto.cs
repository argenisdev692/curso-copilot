namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for logout response
    /// </summary>
    public class LogoutResponseDto
    {
        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if logout was successful
        /// </summary>
        public bool LoggedOut { get; set; }

        /// <summary>
        /// Timestamp of logout
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
