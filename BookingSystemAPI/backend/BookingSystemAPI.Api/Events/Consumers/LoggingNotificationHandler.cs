namespace BookingSystemAPI.Api.Events.Consumers;

/// <summary>
/// Implementación de IBookingNotificationHandler que solo hace logging.
/// Útil para desarrollo y testing. En producción, implementar EmailNotificationHandler, etc.
/// </summary>
public class LoggingNotificationHandler : IBookingNotificationHandler
{
    private readonly ILogger<LoggingNotificationHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del handler de logging.
    /// </summary>
    /// <param name="logger">Logger para trazabilidad.</param>
    public LoggingNotificationHandler(ILogger<LoggingNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<bool> HandleBookingCreatedAsync(BookingCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[NOTIFICATION] Reserva creada - Enviar confirmación. " +
            "BookingId: {BookingId}, Usuario: {UserEmail}, Sala: {RoomName}, " +
            "Inicio: {StartTime:dd/MM/yyyy HH:mm}, Fin: {EndTime:dd/MM/yyyy HH:mm}",
            @event.BookingId,
            @event.UserEmail,
            @event.RoomName,
            @event.StartTime,
            @event.EndTime);

        // Simular envío de notificación
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> HandleBookingCancelledAsync(BookingCancelledEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[NOTIFICATION] Reserva cancelada - Enviar notificación. " +
            "BookingId: {BookingId}, Usuario: {UserEmail}, Sala: {RoomName}, " +
            "Razón: {CancellationReason}",
            @event.BookingId,
            @event.UserEmail,
            @event.RoomName,
            @event.CancellationReason ?? "No especificada");

        // Simular envío de notificación
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> HandleBookingReminderAsync(BookingReminderEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[NOTIFICATION] Recordatorio de reserva - Enviar {ReminderType}. " +
            "BookingId: {BookingId}, Usuario: {UserEmail}, Sala: {RoomName}, " +
            "Inicio en: {MinutesBefore} minutos ({StartTime:dd/MM/yyyy HH:mm})",
            @event.ReminderType,
            @event.BookingId,
            @event.UserEmail,
            @event.RoomName,
            @event.MinutesBefore,
            @event.StartTime);

        // Simular envío de notificación
        return Task.FromResult(true);
    }
}
