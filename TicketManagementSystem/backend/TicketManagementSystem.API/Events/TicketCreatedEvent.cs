namespace TicketManagementSystem.API.Events;

/// <summary>
/// Evento que se publica cuando se crea un nuevo ticket.
/// </summary>
public class TicketCreatedEvent : BaseEvent
{
    /// <summary>
    /// Identificador del ticket creado.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// Título del ticket.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del ticket.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Prioridad del ticket.
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del usuario que creó el ticket.
    /// </summary>
    public int CreatedById { get; set; }

    /// <summary>
    /// Identificador del usuario asignado (puede ser null).
    /// </summary>
    public int? AssignedToId { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de TicketCreatedEvent.
    /// </summary>
    /// <param name="ticketId">Identificador del ticket.</param>
    /// <param name="title">Título del ticket.</param>
    /// <param name="description">Descripción del ticket.</param>
    /// <param name="priority">Prioridad del ticket.</param>
    /// <param name="createdById">Usuario que creó el ticket.</param>
    /// <param name="assignedToId">Usuario asignado (opcional).</param>
    /// <param name="correlationId">Identificador de correlación.</param>
    public TicketCreatedEvent(
        int ticketId,
        string title,
        string? description,
        string priority,
        int createdById,
        int? assignedToId = null,
        string? correlationId = null)
        : base("TicketCreated", correlationId)
    {
        TicketId = ticketId;
        Title = title;
        Description = description;
        Priority = priority;
        CreatedById = createdById;
        AssignedToId = assignedToId;
    }
}