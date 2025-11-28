namespace TicketManagementSystem.API.Events;

/// <summary>
/// Evento que se publica cuando se asigna un ticket a un usuario.
/// </summary>
public class TicketAssignedEvent : BaseEvent
{
    /// <summary>
    /// Identificador del ticket asignado.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// Identificador del usuario asignado.
    /// </summary>
    public int AssignedToId { get; set; }

    /// <summary>
    /// Identificador del usuario que realizó la asignación.
    /// </summary>
    public int AssignedById { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de TicketAssignedEvent.
    /// </summary>
    /// <param name="ticketId">Identificador del ticket.</param>
    /// <param name="assignedToId">Usuario asignado.</param>
    /// <param name="assignedById">Usuario que realizó la asignación.</param>
    /// <param name="correlationId">Identificador de correlación.</param>
    public TicketAssignedEvent(
        int ticketId,
        int assignedToId,
        int assignedById,
        string? correlationId = null)
        : base("TicketAssigned", correlationId)
    {
        TicketId = ticketId;
        AssignedToId = assignedToId;
        AssignedById = assignedById;
    }
}