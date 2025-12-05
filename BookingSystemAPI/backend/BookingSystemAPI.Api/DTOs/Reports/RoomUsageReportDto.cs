namespace BookingSystemAPI.Api.DTOs.Reports;

/// <summary>
/// DTO para el reporte de uso de una sala individual.
/// </summary>
public record RoomUsageReportDto
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
    /// Ubicación de la sala.
    /// </summary>
    public string Location { get; init; } = string.Empty;

    /// <summary>
    /// Capacidad de la sala.
    /// </summary>
    public int Capacity { get; init; }

    /// <summary>
    /// Total de reservas en el período.
    /// </summary>
    public int TotalBookings { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalHours { get; init; }

    /// <summary>
    /// Tasa de ocupación (porcentaje del tiempo disponible que fue reservado).
    /// </summary>
    public double OccupancyRate { get; init; }

    /// <summary>
    /// Promedio de asistentes por reserva.
    /// </summary>
    public double AverageAttendees { get; init; }

    /// <summary>
    /// Usuario que más utilizó la sala.
    /// </summary>
    public string TopUser { get; init; } = string.Empty;

    /// <summary>
    /// Reservas confirmadas.
    /// </summary>
    public int ConfirmedBookings { get; init; }

    /// <summary>
    /// Reservas canceladas.
    /// </summary>
    public int CancelledBookings { get; init; }

    /// <summary>
    /// Reservas completadas.
    /// </summary>
    public int CompletedBookings { get; init; }
}

/// <summary>
/// DTO para el reporte completo de uso de salas.
/// </summary>
public record RoomUsageReportResultDto
{
    /// <summary>
    /// Fecha de inicio del período analizado.
    /// </summary>
    public DateTime PeriodStart { get; init; }

    /// <summary>
    /// Fecha de fin del período analizado.
    /// </summary>
    public DateTime PeriodEnd { get; init; }

    /// <summary>
    /// Fecha de generación del reporte.
    /// </summary>
    public DateTime GeneratedAt { get; init; }

    /// <summary>
    /// Total de salas incluidas en el reporte.
    /// </summary>
    public int TotalRooms { get; init; }

    /// <summary>
    /// Estadísticas por sala.
    /// </summary>
    public IReadOnlyList<RoomUsageReportDto> RoomStats { get; init; } = [];
}
