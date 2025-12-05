namespace BookingSystemAPI.Api.DTOs.Bookings;

/// <summary>
/// DTO para resumen agregado de reservas.
/// </summary>
/// <remarks>
/// Utilizado para reportes y dashboards con datos agregados.
/// </remarks>
public record BookingSummaryDto
{
    /// <summary>
    /// Total de reservas en el período.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Reservas confirmadas.
    /// </summary>
    public int ConfirmedBookings { get; init; }

    /// <summary>
    /// Reservas canceladas.
    /// </summary>
    public int CancelledBookings { get; init; }

    /// <summary>
    /// Reservas pendientes.
    /// </summary>
    public int PendingBookings { get; init; }

    /// <summary>
    /// Reservas completadas.
    /// </summary>
    public int CompletedBookings { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalHoursBooked { get; init; }

    /// <summary>
    /// Promedio de duración en minutos.
    /// </summary>
    public double AverageDurationMinutes { get; init; }

    /// <summary>
    /// Promedio de asistentes por reserva.
    /// </summary>
    public double AverageAttendees { get; init; }

    /// <summary>
    /// Fecha de inicio del período analizado.
    /// </summary>
    public DateTime PeriodStart { get; init; }

    /// <summary>
    /// Fecha de fin del período analizado.
    /// </summary>
    public DateTime PeriodEnd { get; init; }
}

/// <summary>
/// DTO para resumen de reservas por sala.
/// </summary>
public record RoomBookingSummaryDto
{
    /// <summary>
    /// ID de la sala.
    /// </summary>
    public int RoomId { get; init; }

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Total de reservas en la sala.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalHoursBooked { get; init; }

    /// <summary>
    /// Tasa de ocupación (porcentaje).
    /// </summary>
    public double OccupancyRate { get; init; }

    /// <summary>
    /// Capacidad de la sala.
    /// </summary>
    public int RoomCapacity { get; init; }

    /// <summary>
    /// Promedio de asistentes.
    /// </summary>
    public double AverageAttendees { get; init; }

    /// <summary>
    /// Utilización de capacidad promedio (porcentaje).
    /// </summary>
    public double CapacityUtilization { get; init; }
}

/// <summary>
/// DTO para resumen de reservas por organizador.
/// </summary>
public record OrganizerBookingSummaryDto
{
    /// <summary>
    /// Email del organizador.
    /// </summary>
    public string OrganizerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Nombre del organizador.
    /// </summary>
    public string OrganizerName { get; init; } = string.Empty;

    /// <summary>
    /// Total de reservas realizadas.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalHoursBooked { get; init; }

    /// <summary>
    /// Reservas canceladas.
    /// </summary>
    public int CancelledBookings { get; init; }

    /// <summary>
    /// Tasa de cancelación (porcentaje).
    /// </summary>
    public double CancellationRate { get; init; }
}

/// <summary>
/// DTO para estadísticas diarias de reservas.
/// </summary>
public record DailyBookingStatsDto
{
    /// <summary>
    /// Fecha del día.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// Total de reservas del día.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalHoursBooked { get; init; }

    /// <summary>
    /// Número de salas utilizadas.
    /// </summary>
    public int UniqueRoomsUsed { get; init; }

    /// <summary>
    /// Total de asistentes.
    /// </summary>
    public int TotalAttendees { get; init; }
}
