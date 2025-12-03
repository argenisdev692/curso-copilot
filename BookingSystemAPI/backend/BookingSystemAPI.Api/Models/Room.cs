namespace BookingSystemAPI.Api.Models;

/// <summary>
/// Estados posibles de una sala.
/// </summary>
public enum RoomStatus
{
    /// <summary>
    /// La sala está disponible para reservas.
    /// </summary>
    Available = 0,

    /// <summary>
    /// La sala está en mantenimiento y no disponible.
    /// </summary>
    Maintenance = 1
}

/// <summary>
/// Representa una sala o espacio reservable en el sistema.
/// </summary>
public class Room : BaseEntity
{
    /// <summary>
    /// Nombre único de la sala.
    /// </summary>
    /// <example>Sala de Conferencias A</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Capacidad máxima de personas de la sala.
    /// </summary>
    /// <example>20</example>
    public int Capacity { get; set; }

    /// <summary>
    /// Lista de equipamiento disponible en la sala (almacenado como JSON).
    /// </summary>
    /// <example>["Proyector", "Pizarra", "Videoconferencia"]</example>
    public List<string> Equipment { get; set; } = new();

    /// <summary>
    /// Ubicación física de la sala.
    /// </summary>
    /// <example>Edificio Principal, Piso 3</example>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual de la sala.
    /// </summary>
    public RoomStatus Status { get; set; } = RoomStatus.Available;

    /// <summary>
    /// Colección de reservas asociadas a esta sala.
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
