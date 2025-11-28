namespace TicketManagementSystem.API.Events;

/// <summary>
/// Evento que se publica cuando se modifica un ticket y se necesita invalidar el cache.
/// </summary>
public class TicketCacheInvalidationEvent : BaseEvent
{
    /// <summary>
    /// Acción que provocó la invalidación (create, update, delete, assign).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del ticket afectado.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de TicketCacheInvalidationEvent.
    /// </summary>
    /// <param name="action">Acción que provocó la invalidación.</param>
    /// <param name="ticketId">Identificador del ticket.</param>
    /// <param name="correlationId">Identificador de correlación.</param>
    public TicketCacheInvalidationEvent(string action, int ticketId, string correlationId)
    {
        Action = action;
        TicketId = ticketId;
        CorrelationId = correlationId;
    }
}