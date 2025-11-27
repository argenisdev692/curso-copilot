// Modelo TicketHistory para auditoría de cambios en tickets con EF Core
//
// Propiedades:
// - Id: int (PK, Identity)
// - TicketId: int (FK a Ticket.Id, requerido, índice)
// - ChangedById: int (FK a User.Id, requerido, índice)
// - ChangedAt: DateTime (UTC, default now)
// - OldStatus: Status? (estado anterior, nullable)
// - NewStatus: Status (estado nuevo)
// - OldPriority: Priority? (prioridad anterior, nullable)
// - NewPriority: Priority (prioridad nueva)
// - OldAssignedToId: int? (usuario asignado anterior, nullable)
// - NewAssignedToId: int? (usuario asignado nuevo, nullable)
// - ChangeDescription: string? (descripción opcional del cambio)
//
// Relaciones:
// - Ticket: Ticket (ticket al que pertenece el historial)
// - ChangedBy: User (usuario que realizó el cambio)

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystem.API.Models
{
    [Index(nameof(TicketId), nameof(ChangedAt), IsDescending = new[] { false, true })]
    [Index(nameof(ChangedById))]
    public class TicketHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        [Required]
        [ForeignKey("ChangedBy")]
        public int ChangedById { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Status? OldStatus { get; set; }

        public Status NewStatus { get; set; }

        public Priority? OldPriority { get; set; }

        public Priority NewPriority { get; set; }

        public int? OldAssignedToId { get; set; }

        public int? NewAssignedToId { get; set; }

        [MaxLength(500)]
        public string? ChangeDescription { get; set; }

        // Navigation properties
        public virtual Ticket? Ticket { get; set; }
        public virtual User? ChangedBy { get; set; }
    }
}