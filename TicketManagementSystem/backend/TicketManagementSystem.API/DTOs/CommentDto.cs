using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for comment entity
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Comment ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// ID of the user who created the comment
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Name of the user who created the comment
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp in UTC
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp in UTC
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}