using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for creating a new comment
    /// </summary>
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(1000)]
        public required string Content { get; set; }
    }
}