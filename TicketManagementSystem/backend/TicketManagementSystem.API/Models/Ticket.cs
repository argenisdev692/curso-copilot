// Entidad Ticket para el sistema de tickets con EF Core 9
//
// Propiedades:
// - Id: int (PK, Identity)
// - Title: string (máx 200, requerido)
// - Description: string (máx 1000, requerido)
// - Status: Status (enum, default Open)
// - Priority: Priority (enum, default Medium)
// - CreatedById: int (FK a User.Id, requerido, índice)
// - AssignedToId: int? (FK a User.Id, opcional)
// - CreatedAt: DateTime (UTC, default now)
// - UpdatedAt: DateTime (UTC, default now)
//
// Relaciones:
// - CreatedBy: User (usuario que creó el ticket)
// - AssignedTo: User? (usuario asignado, opcional)
// - Comments: colección de comentarios del ticket

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystem.API.Models
{
    [Index(nameof(CreatedById))]
    [Index(nameof(AssignedToId))]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        public Status Status { get; set; } = Status.Open;

        public Priority Priority { get; set; } = Priority.Medium;

        [Required]
        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; }

        [ForeignKey("AssignedTo")]
        public int? AssignedToId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public virtual User? CreatedBy { get; set; }
        public virtual User? AssignedTo { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<TicketHistory>? History { get; set; }
    }
}
