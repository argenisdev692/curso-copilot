using BookingSystemAPI.Api.Common.Responses;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Reports;
using BookingSystemAPI.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador para generación de reportes.
/// Requiere autenticación JWT para todos los endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("general")]
[Tags("📊 Reportes")]
public class ReportsController : BaseApiController
{
    private readonly IRoomReportService _roomReportService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de reportes.
    /// </summary>
    /// <param name="roomReportService">Servicio de reportes de salas.</param>
    /// <param name="logger">Logger para registro de operaciones.</param>
    public ReportsController(IRoomReportService roomReportService, ILogger<ReportsController> logger)
        : base(logger)
    {
        _roomReportService = roomReportService;
    }

    /// <summary>
    /// Obtiene el reporte de uso de salas en formato JSON.
    /// </summary>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="roomId">ID de sala específica (opcional).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="200">Reporte generado exitosamente.</response>
    /// <response code="400">Parámetros inválidos.</response>
    [HttpGet("room-usage")]
    [ProducesResponseType(typeof(ApiResponse<RoomUsageReportResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomUsageReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int? roomId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Solicitando reporte de uso de salas. CorrelationId: {CorrelationId}",
            CorrelationId);

        var query = new RoomUsageQueryDto
        {
            StartDate = startDate,
            EndDate = endDate,
            RoomId = roomId
        };

        var result = await _roomReportService.GetRoomUsageReportAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return Ok(ApiResponse<RoomUsageReportResultDto>.Ok(
            result.Value!,
            "Reporte generado exitosamente",
            CorrelationId));
    }

    /// <summary>
    /// Descarga el reporte de uso de salas en formato Excel.
    /// </summary>
    /// <remarks>
    /// Genera un archivo Excel (.xlsx) con las estadísticas de uso de salas.
    /// El archivo incluye:
    /// - Encabezado con información del período
    /// - Tabla con estadísticas por sala
    /// - Formato profesional con estilos
    /// 
    /// **Límites:**
    /// - Máximo 10,000 filas por reporte
    /// - Período máximo recomendado: 1 año
    /// </remarks>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="roomId">ID de sala específica (opcional).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="200">Archivo Excel generado exitosamente.</response>
    /// <response code="400">Parámetros inválidos.</response>
    [HttpGet("room-usage/excel")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadRoomUsageExcel(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int? roomId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Descargando Excel de uso de salas. CorrelationId: {CorrelationId}, Período: {StartDate} - {EndDate}",
            CorrelationId, startDate, endDate);

        var query = new RoomUsageQueryDto
        {
            StartDate = startDate,
            EndDate = endDate,
            RoomId = roomId
        };

        var result = await _roomReportService.GenerateRoomUsageExcelAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        var fileName = $"room-usage-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

        // Configurar headers para descarga
        Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
        Response.Headers.Append("X-Correlation-Id", CorrelationId);

        return File(
            result.Value!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    /// <summary>
    /// Convierte un error en una respuesta HTTP apropiada.
    /// </summary>
    /// <param name="error">Error a convertir.</param>
    /// <returns>Resultado HTTP.</returns>
    private IActionResult ToErrorResult(Error error)
    {
        var statusCode = error.Code switch
        {
            "NotFound" => StatusCodes.Status404NotFound,
            "Conflict" => StatusCodes.Status409Conflict,
            "Validation" => StatusCodes.Status400BadRequest,
            "DateRange" => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status422UnprocessableEntity
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code switch
            {
                "NotFound" => "Recurso no encontrado",
                "Conflict" => "Conflicto",
                "Validation" => "Error de validación",
                "DateRange" => "Rango de fechas inválido",
                _ => "Error"
            },
            Detail = error.Message,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["correlationId"] = CorrelationId;

        return StatusCode(statusCode, problemDetails);
    }
}
