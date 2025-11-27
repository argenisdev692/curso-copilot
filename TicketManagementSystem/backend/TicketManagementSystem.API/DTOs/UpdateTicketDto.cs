using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO for updating an existing ticket
    /// </summary>
    public class UpdateTicketDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public Status? Status { get; set; }

        public Priority? Priority { get; set; }

        public int? AssignedToId { get; set; }
    }
}