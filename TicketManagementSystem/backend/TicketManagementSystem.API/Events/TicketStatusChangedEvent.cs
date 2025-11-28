namespace TicketManagementSystem.API.Events;

/// <summary>
/// Evento que se publica cuando cambia el estado de un ticket.
/// </summary>
public class TicketStatusChangedEvent : BaseEvent
{
    /// <summary>
    /// Identificador del ticket.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// Estado anterior del ticket.
    /// </summary>
    public string OldStatus { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo estado del ticket.
    /// </summary>
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del usuario que cambió el estado.
    /// </summary>
    public int ChangedById { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de TicketStatusChangedEvent.
    /// </summary>
    /// <param name="ticketId">Identificador del ticket.</param>
    /// <param name="oldStatus">Estado anterior.</param>
    /// <param name="newStatus">Nuevo estado.</param>
    /// <param name="changedById">Usuario que cambió el estado.</param>
    /// <param name="correlationId">Identificador de correlación.</param>
    public TicketStatusChangedEvent(
        int ticketId,
        string oldStatus,
        string newStatus,
        int changedById,
        string? correlationId = null)
        : base("TicketStatusChanged", correlationId)
    {
        TicketId = ticketId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedById = changedById;
    }
}