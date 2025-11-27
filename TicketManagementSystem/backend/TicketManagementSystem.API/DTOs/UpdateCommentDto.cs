using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for updating an existing comment
    /// </summary>
    public class UpdateCommentDto
    {
        [Required]
        [MaxLength(1000)]
        public required string Content { get; set; }
    }
}