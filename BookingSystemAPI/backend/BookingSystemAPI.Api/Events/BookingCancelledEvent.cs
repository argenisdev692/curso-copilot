namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Evento publicado cuando se cancela una reserva.
/// </summary>
public class BookingCancelledEvent : BaseEvent
{
    /// <summary>
    /// Identificador de la reserva cancelada.
    /// </summary>
    public int BookingId { get; init; }

    /// <summary>
    /// Título de la reserva cancelada.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Identificador de la sala.
    /// </summary>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Identificador del usuario propietario de la reserva.
    /// </summary>
    public int UserId { get; init; }

    /// <summary>
    /// Email del usuario para notificaciones.
    /// </summary>
    public string UserEmail { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de inicio de la reserva cancelada.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin de la reserva cancelada.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Razón de la cancelación (opcional).
    /// </summary>
    public string? CancellationReason { get; init; }

    /// <summary>
    /// Usuario que realizó la cancelación (puede ser diferente al propietario).
    /// </summary>
    public int? CancelledByUserId { get; init; }
}
