using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.DTOs.Reports;

/// <summary>
/// DTO para los parámetros de consulta del reporte de uso de salas.
/// </summary>
public record RoomUsageQueryDto
{
    /// <summary>
    /// Fecha de inicio del período a analizar.
    /// </summary>
    /// <example>2025-01-01</example>
    [Required(ErrorMessage = "La fecha de inicio es requerida.")]
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Fecha de fin del período a analizar.
    /// </summary>
    /// <example>2025-12-31</example>
    [Required(ErrorMessage = "La fecha de fin es requerida.")]
    public DateTime EndDate { get; init; }

    /// <summary>
    /// ID de sala específica (opcional). Si no se especifica, incluye todas las salas.
    /// </summary>
    /// <example>1</example>
    public int? RoomId { get; init; }
}
