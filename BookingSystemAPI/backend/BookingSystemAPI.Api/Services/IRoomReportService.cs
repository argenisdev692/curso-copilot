using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Reports;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Servicio para generación de reportes de uso de salas.
/// </summary>
public interface IRoomReportService
{
    /// <summary>
    /// Obtiene los datos del reporte de uso de salas.
    /// </summary>
    /// <param name="query">Parámetros de consulta del reporte.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado con los datos del reporte.</returns>
    Task<Result<RoomUsageReportResultDto>> GetRoomUsageReportAsync(
        RoomUsageQueryDto query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Genera un archivo Excel con el reporte de uso de salas.
    /// </summary>
    /// <param name="query">Parámetros de consulta del reporte.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Stream con el archivo Excel.</returns>
    Task<Result<Stream>> GenerateRoomUsageExcelAsync(
        RoomUsageQueryDto query,
        CancellationToken cancellationToken = default);
}
