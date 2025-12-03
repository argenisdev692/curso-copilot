namespace BookingSystemAPI.Api.Models;

/// <summary>
/// Estados posibles de una reserva.
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Reserva pendiente de confirmación.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Reserva confirmada.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Reserva cancelada.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Reserva completada.
    /// </summary>
    Completed = 3
}

/// <summary>
/// Representa una reserva de sala en el sistema.
/// </summary>
public class Booking : BaseEntity
{
    /// <summary>
    /// Título o descripción breve de la reserva.
    /// </summary>
    /// <example>Reunión de equipo</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de la reserva.
    /// </summary>
    /// <example>Reunión semanal del equipo de desarrollo</example>
    public string? Description { get; set; }

    /// <summary>
    /// Fecha y hora de inicio de la reserva (UTC).
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Fecha y hora de fin de la reserva (UTC).
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Identificador de la sala reservada.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Sala reservada (navegación).
    /// </summary>
    public virtual Room Room { get; set; } = null!;

    /// <summary>
    /// Nombre del organizador de la reserva.
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// Email del organizador.
    /// </summary>
    public string OrganizerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Número de asistentes esperados.
    /// </summary>
    public int AttendeeCount { get; set; }

    /// <summary>
    /// Estado actual de la reserva.
    /// </summary>
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// Notas adicionales de la reserva.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Calcula la duración de la reserva.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;
}
