namespace BookingSystemAPI.Api.Events.Consumers;

/// <summary>
/// Interfaz para manejar notificaciones de reservas.
/// Permite diferentes implementaciones (Email, SMS, Push, etc.).
/// </summary>
public interface IBookingNotificationHandler
{
    /// <summary>
    /// Envía notificación de reserva creada.
    /// </summary>
    /// <param name="event">Evento de reserva creada.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la notificación se envió correctamente.</returns>
    Task<bool> HandleBookingCreatedAsync(BookingCreatedEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Envía notificación de reserva cancelada.
    /// </summary>
    /// <param name="event">Evento de reserva cancelada.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la notificación se envió correctamente.</returns>
    Task<bool> HandleBookingCancelledAsync(BookingCancelledEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Envía recordatorio de reserva.
    /// </summary>
    /// <param name="event">Evento de recordatorio.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la notificación se envió correctamente.</returns>
    Task<bool> HandleBookingReminderAsync(BookingReminderEvent @event, CancellationToken cancellationToken = default);
}
