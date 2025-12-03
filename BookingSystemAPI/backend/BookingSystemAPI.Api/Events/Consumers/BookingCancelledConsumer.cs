using MassTransit;

namespace BookingSystemAPI.Api.Events.Consumers;

/// <summary>
/// Consumer para eventos de reserva cancelada.
/// Procesa el evento y delega la notificación al handler correspondiente.
/// </summary>
public class BookingCancelledConsumer : IConsumer<BookingCancelledEvent>
{
    private readonly IBookingNotificationHandler _notificationHandler;
    private readonly ILogger<BookingCancelledConsumer> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del consumer.
    /// </summary>
    /// <param name="notificationHandler">Handler de notificaciones.</param>
    /// <param name="logger">Logger para trazabilidad.</param>
    public BookingCancelledConsumer(
        IBookingNotificationHandler notificationHandler,
        ILogger<BookingCancelledConsumer> logger)
    {
        _notificationHandler = notificationHandler;
        _logger = logger;
    }

    /// <summary>
    /// Consume el evento de reserva cancelada.
    /// </summary>
    /// <param name="context">Contexto del mensaje.</param>
    public async Task Consume(ConsumeContext<BookingCancelledEvent> context)
    {
        var @event = context.Message;
        var correlationId = context.CorrelationId?.ToString() ?? @event.CorrelationId;

        _logger.LogInformation(
            "[CONSUMER] Procesando BookingCancelledEvent. BookingId: {BookingId}, CorrelationId: {CorrelationId}",
            @event.BookingId, correlationId);

        try
        {
            var result = await _notificationHandler.HandleBookingCancelledAsync(@event, context.CancellationToken);

            if (result)
            {
                _logger.LogInformation(
                    "[CONSUMER] Notificación de reserva cancelada enviada exitosamente. BookingId: {BookingId}",
                    @event.BookingId);
            }
            else
            {
                _logger.LogWarning(
                    "[CONSUMER] No se pudo enviar notificación de reserva cancelada. BookingId: {BookingId}",
                    @event.BookingId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[CONSUMER] Error al procesar BookingCancelledEvent. BookingId: {BookingId}, CorrelationId: {CorrelationId}",
                @event.BookingId, correlationId);
            throw; // Re-throw para que MassTransit maneje el retry
        }
    }
}
