using System;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Read model optimizado para queries de tickets, con proyecciones directas para mejor performance
    /// </summary>
    public class TicketReadModel
    {
        /// <summary>
        /// Identificador único del ticket
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título del ticket
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del ticket
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual del ticket
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Nombre de la prioridad (proyectado directamente)
        /// </summary>
        public string PriorityName { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario creador (proyectado directamente)
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario asignado (puede ser null)
        /// </summary>
        public string? AssignedToName { get; set; }

        /// <summary>
        /// Cantidad de comentarios (calculado en DB)
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}