namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Evento publicado cuando se crea una nueva reserva.
/// </summary>
public class BookingCreatedEvent : BaseEvent
{
    /// <summary>
    /// Identificador de la reserva creada.
    /// </summary>
    public int BookingId { get; init; }

    /// <summary>
    /// Título de la reserva.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Descripción de la reserva.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Identificador de la sala reservada.
    /// </summary>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre de la sala reservada.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Identificador del usuario que creó la reserva.
    /// </summary>
    public int UserId { get; init; }

    /// <summary>
    /// Email del usuario para notificaciones.
    /// </summary>
    public string UserEmail { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de inicio de la reserva.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin de la reserva.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Número de asistentes.
    /// </summary>
    public int AttendeeCount { get; init; }

    /// <summary>
    /// Estado de la reserva.
    /// </summary>
    public string Status { get; init; } = "Confirmed";
}
