namespace BookingSystemAPI.Api.DTOs.Bookings;

/// <summary>
/// DTO para filtros de búsqueda de reservas con soporte para paginación.
/// </summary>
/// <remarks>
/// Proporciona parámetros opcionales para consultas flexibles sin requerir todos los campos.
/// </remarks>
public record BookingQueryDto
{
    /// <summary>
    /// ID del usuario organizador (opcional).
    /// </summary>
    /// <example>1</example>
    public int? UserId { get; init; }

    /// <summary>
    /// ID de la sala (opcional).
    /// </summary>
    /// <example>2</example>
    public int? RoomId { get; init; }

    /// <summary>
    /// Fecha de inicio del rango de búsqueda (opcional).
    /// </summary>
    /// <example>2025-01-01T00:00:00Z</example>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Fecha de fin del rango de búsqueda (opcional).
    /// </summary>
    /// <example>2025-12-31T23:59:59Z</example>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Estado de la reserva para filtrar (opcional).
    /// </summary>
    /// <example>Confirmed</example>
    public string? Status { get; init; }

    /// <summary>
    /// Email del organizador para filtrar (opcional).
    /// </summary>
    /// <example>usuario@ejemplo.com</example>
    public string? OrganizerEmail { get; init; }

    /// <summary>
    /// Texto de búsqueda para título o descripción (opcional).
    /// </summary>
    /// <example>reunión</example>
    public string? SearchText { get; init; }

    /// <summary>
    /// Número de página para paginación (1-indexed).
    /// </summary>
    /// <example>1</example>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    /// <example>20</example>
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Campo por el cual ordenar.
    /// </summary>
    /// <example>StartTime</example>
    public string? SortBy { get; init; }

    /// <summary>
    /// Dirección del ordenamiento (asc/desc).
    /// </summary>
    /// <example>desc</example>
    public string SortDirection { get; init; } = "desc";

    /// <summary>
    /// Calcula el número de elementos a saltar para paginación.
    /// </summary>
    public int Skip => (Page - 1) * PageSize;

    /// <summary>
    /// Indica si el ordenamiento es descendente.
    /// </summary>
    public bool IsDescending => SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// DTO para respuesta paginada de reservas.
/// </summary>
/// <typeparam name="T">Tipo de elemento en la colección.</typeparam>
public record PagedResultDto<T>
{
    /// <summary>
    /// Elementos de la página actual.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>
    /// Número total de elementos.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Número de página actual.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Tamaño de página.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Número total de páginas.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
}

/// <summary>
/// DTO simplificado para resultados de búsqueda de reservas.
/// </summary>
public record BookingSearchResultDto
{
    /// <summary>
    /// ID de la reserva.
    /// </summary>
    public int BookingId { get; init; }

    /// <summary>
    /// Título de la reserva.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Nombre del usuario organizador.
    /// </summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// Nombre de la sala.
    /// </summary>
    public string RoomName { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de inicio.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Fecha y hora de fin.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Precio total calculado (si aplica).
    /// </summary>
    public decimal? TotalPrice { get; init; }

    /// <summary>
    /// Estado de la reserva.
    /// </summary>
    public string Status { get; init; } = string.Empty;
}
