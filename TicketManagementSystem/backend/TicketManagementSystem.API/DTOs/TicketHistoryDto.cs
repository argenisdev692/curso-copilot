using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Data Transfer Object para el historial de cambios de un ticket.
    /// Incluye información enriquecida con nombres de usuarios para evitar N+1 queries.
    /// </summary>
    public record TicketHistoryDto
    {
        /// <summary>
        /// Identificador único del registro de historial
        /// </summary>
        /// <example>1</example>
        public int Id { get; init; }

        /// <summary>
        /// ID del ticket al que pertenece este registro de historial
        /// </summary>
        /// <example>42</example>
        public int TicketId { get; init; }

        /// <summary>
        /// ID del usuario que realizó el cambio
        /// </summary>
        /// <example>5</example>
        public int ChangedById { get; init; }

        /// <summary>
        /// Nombre completo del usuario que realizó el cambio
        /// </summary>
        /// <example>Juan García</example>
        public string ChangedByName { get; init; } = string.Empty;

        /// <summary>
        /// Email del usuario que realizó el cambio
        /// </summary>
        /// <example>juan.garcia@example.com</example>
        public string? ChangedByEmail { get; init; }

        /// <summary>
        /// Fecha y hora en UTC cuando se realizó el cambio
        /// </summary>
        /// <example>2025-11-25T14:32:00Z</example>
        public DateTime ChangedAt { get; init; }

        /// <summary>
        /// Estado anterior del ticket (null si es creación)
        /// </summary>
        /// <example>Open</example>
        public string? OldStatus { get; init; }

        /// <summary>
        /// Nuevo estado del ticket
        /// </summary>
        /// <example>InProgress</example>
        public string NewStatus { get; init; } = string.Empty;

        /// <summary>
        /// Prioridad anterior del ticket (null si es creación)
        /// </summary>
        /// <example>Medium</example>
        public string? OldPriority { get; init; }

        /// <summary>
        /// Nueva prioridad del ticket
        /// </summary>
        /// <example>High</example>
        public string NewPriority { get; init; } = string.Empty;

        /// <summary>
        /// ID del usuario asignado anteriormente (null si no había asignación)
        /// </summary>
        /// <example>3</example>
        public int? OldAssignedToId { get; init; }

        /// <summary>
        /// Nombre del usuario asignado anteriormente
        /// </summary>
        /// <example>María López</example>
        public string? OldAssignedToName { get; init; }

        /// <summary>
        /// ID del nuevo usuario asignado (null si se quitó la asignación)
        /// </summary>
        /// <example>7</example>
        public int? NewAssignedToId { get; init; }

        /// <summary>
        /// Nombre del nuevo usuario asignado
        /// </summary>
        /// <example>Carlos Ruiz</example>
        public string? NewAssignedToName { get; init; }

        /// <summary>
        /// Descripción opcional del cambio realizado
        /// </summary>
        /// <example>Escalado por urgencia del cliente</example>
        public string? ChangeDescription { get; init; }

        /// <summary>
        /// Indica si este registro representa la creación del ticket
        /// </summary>
        /// <example>false</example>
        public bool IsCreation { get; init; }

        /// <summary>
        /// Lista de cambios específicos detectados en este registro
        /// </summary>
        public List<TicketHistoryChangeDto> Changes { get; init; } = new();
    }

    /// <summary>
    /// Representa un cambio específico dentro de un registro de historial
    /// </summary>
    public record TicketHistoryChangeDto
    {
        /// <summary>
        /// Campo que fue modificado
        /// </summary>
        /// <example>Status</example>
        public string Field { get; init; } = string.Empty;

        /// <summary>
        /// Valor anterior del campo
        /// </summary>
        /// <example>Open</example>
        public string? OldValue { get; init; }

        /// <summary>
        /// Nuevo valor del campo
        /// </summary>
        /// <example>InProgress</example>
        public string? NewValue { get; init; }

        /// <summary>
        /// Valor anterior formateado para mostrar (ej: nombre de usuario en lugar de ID)
        /// </summary>
        /// <example>Sin asignar</example>
        public string? OldDisplayValue { get; init; }

        /// <summary>
        /// Nuevo valor formateado para mostrar
        /// </summary>
        /// <example>Juan García</example>
        public string? NewDisplayValue { get; init; }
    }

    /// <summary>
    /// Parámetros de consulta para filtrar el historial de tickets
    /// </summary>
    public record TicketHistoryFilterDto
    {
        /// <summary>
        /// Fecha de inicio para filtrar (inclusive)
        /// </summary>
        public DateTime? FromDate { get; init; }

        /// <summary>
        /// Fecha de fin para filtrar (inclusive)
        /// </summary>
        public DateTime? ToDate { get; init; }

        /// <summary>
        /// Filtrar por ID del usuario que realizó los cambios
        /// </summary>
        public int? ChangedById { get; init; }

        /// <summary>
        /// Número de página (1-indexed)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; init; } = 1;

        /// <summary>
        /// Tamaño de página
        /// </summary>
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int PageSize { get; init; } = 20;
    }
}
