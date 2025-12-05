namespace BookingSystemAPI.Api.DTOs.Rooms;

/// <summary>
/// DTO para consulta de disponibilidad de salas.
/// </summary>
public record RoomAvailabilityQueryDto
{
    /// <summary>
    /// Fecha y hora de inicio requerida.
    /// </summary>
    /// <example>2025-12-10T09:00:00Z</example>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin requerida.
    /// </summary>
    /// <example>2025-12-10T10:00:00Z</example>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Capacidad mínima requerida (opcional).
    /// </summary>
    /// <example>10</example>
    public int? MinCapacity { get; init; }

    /// <summary>
    /// Equipamiento requerido (opcional).
    /// </summary>
    /// <example>["Proyector", "Videoconferencia"]</example>
    public List<string>? RequiredEquipment { get; init; }

    /// <summary>
    /// Ubicación preferida (opcional).
    /// </summary>
    /// <example>Edificio Principal</example>
    public string? PreferredLocation { get; init; }
}

/// <summary>
/// DTO para sala disponible con detalles.
/// </summary>
public record AvailableRoomDto
{
    /// <summary>
    /// ID de la sala.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Capacidad de la sala.
    /// </summary>
    public int Capacity { get; init; }

    /// <summary>
    /// Ubicación de la sala.
    /// </summary>
    public string Location { get; init; } = string.Empty;

    /// <summary>
    /// Equipamiento disponible.
    /// </summary>
    public List<string> Equipment { get; init; } = new();

    /// <summary>
    /// Indica si cumple con todo el equipamiento requerido.
    /// </summary>
    public bool HasAllRequiredEquipment { get; init; }

    /// <summary>
    /// Próxima reserva programada (si existe).
    /// </summary>
    public DateTime? NextBookingStart { get; init; }
}

/// <summary>
/// DTO para ocupación de sala.
/// </summary>
public record RoomOccupancyDto
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
    /// Total de horas disponibles en el período.
    /// </summary>
    public double TotalAvailableHours { get; init; }

    /// <summary>
    /// Total de horas reservadas.
    /// </summary>
    public double TotalBookedHours { get; init; }

    /// <summary>
    /// Porcentaje de ocupación.
    /// </summary>
    public double OccupancyPercentage { get; init; }

    /// <summary>
    /// Número de reservas en el período.
    /// </summary>
    public int BookingCount { get; init; }

    /// <summary>
    /// Fecha de inicio del período.
    /// </summary>
    public DateTime PeriodStart { get; init; }

    /// <summary>
    /// Fecha de fin del período.
    /// </summary>
    public DateTime PeriodEnd { get; init; }
}
