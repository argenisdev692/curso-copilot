namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Evento publicado para enviar recordatorios de reservas próximas.
/// </summary>
public class BookingReminderEvent : BaseEvent
{
    /// <summary>
    /// Identificador de la reserva.
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
    /// Identificador de la sala.
    /// </summary>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Ubicación de la sala.
    /// </summary>
    public string? RoomLocation { get; init; }

    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public int UserId { get; init; }

    /// <summary>
    /// Email del usuario para la notificación.
    /// </summary>
    public string UserEmail { get; init; } = string.Empty;

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de inicio de la reserva.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin de la reserva.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Minutos antes de la reserva para el recordatorio.
    /// </summary>
    public int MinutesBefore { get; init; }

    /// <summary>
    /// Tipo de recordatorio (Email, Push, SMS).
    /// </summary>
    public ReminderType ReminderType { get; init; } = ReminderType.Email;
}

/// <summary>
/// Tipos de recordatorio disponibles.
/// </summary>
public enum ReminderType
{
    /// <summary>
    /// Recordatorio por correo electrónico.
    /// </summary>
    Email,

    /// <summary>
    /// Notificación push.
    /// </summary>
    Push,

    /// <summary>
    /// Mensaje SMS.
    /// </summary>
    Sms
}
