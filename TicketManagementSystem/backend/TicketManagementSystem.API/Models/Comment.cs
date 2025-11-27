// Modelo Comment para el sistema de tickets con EF Core
//
// Propiedades:
// - Id: int (PK, Identity)
// - Content: string (máx 1000, requerido)
// - CreatedById: int (FK a User.Id, requerido, índice)
// - TicketId: int (FK a Ticket.Id, requerido, índice)
// - CreatedAt: DateTime (UTC, default now)
// - UpdatedAt: DateTime (UTC, default now)
//
// Relaciones:
// - CreatedBy: User (usuario que creó el comentario)
// - Ticket: Ticket (ticket al que pertenece el comentario)

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystem.API.Models
{
    [Index(nameof(CreatedById))]
    [Index(nameof(TicketId))]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Content { get; set; }

        [Required]
        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public virtual User? CreatedBy { get; set; }
        public virtual Ticket? Ticket { get; set; }
    }
}
