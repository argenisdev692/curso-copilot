using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for logout request
    /// </summary>
    public class LogoutDto
    {
        /// <summary>
        /// Refresh token to invalidate
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required")]
        public required string RefreshToken { get; set; }
    }
}
