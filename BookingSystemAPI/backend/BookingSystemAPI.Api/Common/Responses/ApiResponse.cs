using System.Text.Json.Serialization;

namespace BookingSystemAPI.Api.Common.Responses;

/// <summary>
/// Respuesta estándar de la API para operaciones exitosas.
/// </summary>
/// <typeparam name="T">Tipo de datos contenidos en la respuesta.</typeparam>
public record ApiResponse<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Mensaje descriptivo de la operación.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Datos retornados por la operación.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Marca de tiempo de la respuesta.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador de correlación para trazabilidad.
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Crea una respuesta exitosa.
    /// </summary>
    /// <param name="data">Datos a incluir en la respuesta.</param>
    /// <param name="message">Mensaje opcional.</param>
    /// <param name="correlationId">ID de correlación opcional.</param>
    /// <returns>Respuesta exitosa.</returns>
    public static ApiResponse<T> Ok(T data, string? message = null, string? correlationId = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message,
            CorrelationId = correlationId
        };

    /// <summary>
    /// Crea una respuesta de error.
    /// </summary>
    /// <param name="message">Mensaje de error.</param>
    /// <param name="correlationId">ID de correlación opcional.</param>
    /// <returns>Respuesta de error.</returns>
    public static ApiResponse<T> Fail(string message, string? correlationId = null) =>
        new()
        {
            Success = false,
            Message = message,
            CorrelationId = correlationId
        };
}

/// <summary>
/// Respuesta paginada de la API.
/// </summary>
/// <typeparam name="T">Tipo de elementos en la colección.</typeparam>
public record PagedResponse<T> : ApiResponse<IEnumerable<T>>
{
    /// <summary>
    /// Número de página actual (base 1).
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Tamaño de página.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Total de elementos disponibles.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Total de páginas disponibles.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Indica si hay página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indica si hay página siguiente.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}
