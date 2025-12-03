namespace BookingSystemAPI.Api.DTOs.Bookings;

/// <summary>
/// DTO de respuesta para una reserva.
/// </summary>
public record BookingDto : BaseDto
{
    /// <summary>
    /// Título de la reserva.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Descripción de la reserva.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Fecha y hora de inicio.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Duración en minutos.
    /// </summary>
    public int DurationMinutes { get; init; }

    /// <summary>
    /// ID de la sala.
    /// </summary>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Nombre del organizador.
    /// </summary>
    public string OrganizerName { get; init; } = string.Empty;

    /// <summary>
    /// Email del organizador.
    /// </summary>
    public string OrganizerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Número de asistentes.
    /// </summary>
    public int AttendeeCount { get; init; }

    /// <summary>
    /// Estado de la reserva.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Notas adicionales.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// DTO para crear una nueva reserva.
/// </summary>
public record CreateBookingDto : CreateBaseDto
{
    /// <summary>
    /// Título de la reserva.
    /// </summary>
    /// <example>Reunión de planificación</example>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Descripción opcional.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Fecha y hora de inicio (UTC).
    /// </summary>
    /// <example>2025-12-10T09:00:00Z</example>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin (UTC).
    /// </summary>
    /// <example>2025-12-10T10:00:00Z</example>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// ID de la sala a reservar.
    /// </summary>
    /// <example>1</example>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre del organizador.
    /// </summary>
    /// <example>Juan Pérez</example>
    public string OrganizerName { get; init; } = string.Empty;

    /// <summary>
    /// Email del organizador.
    /// </summary>
    /// <example>juan.perez@empresa.com</example>
    public string OrganizerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Número esperado de asistentes.
    /// </summary>
    /// <example>10</example>
    public int AttendeeCount { get; init; }

    /// <summary>
    /// Notas adicionales.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// DTO para actualizar una reserva existente.
/// </summary>
public record UpdateBookingDto : UpdateBaseDto
{
    /// <summary>
    /// Título de la reserva.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Descripción opcional.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Nueva fecha y hora de inicio (UTC).
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Nueva fecha y hora de fin (UTC).
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Nuevo número de asistentes.
    /// </summary>
    public int AttendeeCount { get; init; }

    /// <summary>
    /// Notas adicionales.
    /// </summary>
    public string? Notes { get; init; }
}
