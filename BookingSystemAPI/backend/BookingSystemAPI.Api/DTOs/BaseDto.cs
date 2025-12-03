namespace BookingSystemAPI.Api.DTOs;

/// <summary>
/// DTO base con propiedades comunes de respuesta.
/// </summary>
public abstract record BaseDto
{
    /// <summary>
    /// Identificador único del recurso.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Fecha de creación del recurso.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha de última actualización del recurso.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// DTO base para operaciones de creación.
/// </summary>
public abstract record CreateBaseDto;

/// <summary>
/// DTO base para operaciones de actualización.
/// </summary>
public abstract record UpdateBaseDto;
