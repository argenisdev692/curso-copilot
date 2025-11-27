using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for creating a new ticket
    /// </summary>
    public class CreateTicketDto
    {
        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        [Required]
        public Priority Priority { get; set; } = Priority.Medium;

        public int? AssignedToId { get; set; }
    }
}