namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Basic user information DTO
    /// </summary>
    public class UserBasicDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User role
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}