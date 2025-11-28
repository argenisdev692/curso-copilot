using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text.Json;
using TicketManagementSystem.API.Events;
using TicketManagementSystem.API.Infrastructure.RabbitMQ;

namespace TicketManagementSystem.API.Consumers;

/// <summary>
/// Consumidor de notificaciones para eventos de tickets.
/// Procesa eventos como TicketCreated, TicketAssigned y TicketStatusChanged.
/// </summary>
public class NotificationConsumer : BaseConsumer
{
    /// <summary>
    /// Inicializa una nueva instancia de NotificationConsumer.
    /// </summary>
    /// <param name="connection">Conexión a RabbitMQ.</param>
    /// <param name="logger">Logger para registrar eventos.</param>
    public NotificationConsumer(IRabbitMQConnection connection, ILogger<NotificationConsumer> logger)
        : base(connection, logger, "notification-queue")
    {
    }

    /// <summary>
    /// Procesa el contenido específico del mensaje de notificación.
    /// </summary>
    /// <param name="message">Contenido del mensaje en formato JSON.</param>
    /// <param name="properties">Propiedades del mensaje.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    protected override async Task ProcessMessageCoreAsync(string message, IBasicProperties properties)
    {
        // Deserializar el evento base para determinar el tipo
        var baseEvent = JsonSerializer.Deserialize<BaseEvent>(message);

        if (baseEvent == null)
        {
            _logger.LogWarning("Mensaje no válido recibido: {Message}", message);
            return;
        }

        // Procesar según el tipo de evento
        switch (baseEvent.EventType)
        {
            case "TicketCreated":
                await ProcessTicketCreatedEventAsync(message);
                break;

            case "TicketAssigned":
                await ProcessTicketAssignedEventAsync(message);
                break;

            case "TicketStatusChanged":
                await ProcessTicketStatusChangedEventAsync(message);
                break;

            default:
                _logger.LogWarning("Tipo de evento no reconocido: {EventType}", baseEvent.EventType);
                break;
        }
    }

    /// <summary>
    /// Procesa un evento de ticket creado.
    /// </summary>
    /// <param name="message">Mensaje JSON del evento.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    private async Task ProcessTicketCreatedEventAsync(string message)
    {
        var ticketEvent = JsonSerializer.Deserialize<TicketCreatedEvent>(message);

        if (ticketEvent == null)
        {
            _logger.LogWarning("Error deserializando TicketCreatedEvent: {Message}", message);
            return;
        }

        _logger.LogInformation("Procesando creación de ticket {TicketId}: {Title}",
            ticketEvent.TicketId, ticketEvent.Title);

        // Aquí iría la lógica real de notificación (email, Slack, etc.)
        // Por ahora, solo simulamos el procesamiento
        await Task.Delay(100); // Simular procesamiento

        _logger.LogInformation("Notificación enviada por creación de ticket {TicketId}", ticketEvent.TicketId);
    }

    /// <summary>
    /// Procesa un evento de ticket asignado.
    /// </summary>
    /// <param name="message">Mensaje JSON del evento.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    private async Task ProcessTicketAssignedEventAsync(string message)
    {
        var ticketEvent = JsonSerializer.Deserialize<TicketAssignedEvent>(message);

        if (ticketEvent == null)
        {
            _logger.LogWarning("Error deserializando TicketAssignedEvent: {Message}", message);
            return;
        }

        _logger.LogInformation("Procesando asignación de ticket {TicketId} a usuario {AssignedToId}",
            ticketEvent.TicketId, ticketEvent.AssignedToId);

        // Lógica de notificación
        await Task.Delay(100);

        _logger.LogInformation("Notificación enviada por asignación de ticket {TicketId}", ticketEvent.TicketId);
    }

    /// <summary>
    /// Procesa un evento de cambio de estado de ticket.
    /// </summary>
    /// <param name="message">Mensaje JSON del evento.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    private async Task ProcessTicketStatusChangedEventAsync(string message)
    {
        var ticketEvent = JsonSerializer.Deserialize<TicketStatusChangedEvent>(message);

        if (ticketEvent == null)
        {
            _logger.LogWarning("Error deserializando TicketStatusChangedEvent: {Message}", message);
            return;
        }

        _logger.LogInformation("Procesando cambio de estado de ticket {TicketId}: {OldStatus} -> {NewStatus}",
            ticketEvent.TicketId, ticketEvent.OldStatus, ticketEvent.NewStatus);

        // Lógica de notificación
        await Task.Delay(100);

        _logger.LogInformation("Notificación enviada por cambio de estado de ticket {TicketId}", ticketEvent.TicketId);
    }
}