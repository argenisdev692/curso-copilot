namespace BookingSystemAPI.Api.DTOs.Rooms;

/// <summary>
/// DTO de respuesta para Room.
/// </summary>
public record RoomDto
{
    /// <summary>ID de la sala.</summary>
    public int Id { get; init; }

    /// <summary>Nombre de la sala.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Capacidad máxima de personas.</summary>
    public int Capacity { get; init; }

    /// <summary>Lista de equipamiento disponible.</summary>
    public List<string> Equipment { get; init; } = [];

    /// <summary>Ubicación de la sala.</summary>
    public string Location { get; init; } = string.Empty;

    /// <summary>Estado de la sala.</summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>Fecha de creación.</summary>
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// DTO para crear una nueva sala.
/// </summary>
public record CreateRoomDto
{
    /// <summary>Nombre único de la sala.</summary>
    /// <example>Sala de Conferencias A</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>Capacidad máxima de personas.</summary>
    /// <example>20</example>
    public int Capacity { get; init; }

    /// <summary>Lista de equipamiento disponible.</summary>
    /// <example>["Proyector", "Pizarra", "Videoconferencia"]</example>
    public List<string> Equipment { get; init; } = [];

    /// <summary>Ubicación de la sala.</summary>
    /// <example>Edificio Principal, Piso 3</example>
    public string Location { get; init; } = string.Empty;
}

/// <summary>
/// DTO para actualizar una sala existente.
/// </summary>
public record UpdateRoomDto
{
    /// <summary>Nombre único de la sala.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Capacidad máxima de personas.</summary>
    public int Capacity { get; init; }

    /// <summary>Lista de equipamiento disponible.</summary>
    public List<string> Equipment { get; init; } = [];

    /// <summary>Ubicación de la sala.</summary>
    public string Location { get; init; } = string.Empty;

    /// <summary>Estado de la sala (Available, Maintenance).</summary>
    public string Status { get; init; } = "Available";
}
