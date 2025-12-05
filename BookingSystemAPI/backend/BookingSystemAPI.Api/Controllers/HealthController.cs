using Microsoft.AspNetCore.Mvc;
using BookingSystemAPI.Api.Common.Responses;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador para verificación de salud y estado de la API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("❤️ Health")]
public class HealthController : BaseApiController
{
    /// <summary>
    /// Inicializa una nueva instancia del controlador.
    /// </summary>
    /// <param name="logger">Logger para registro.</param>
    public HealthController(ILogger<HealthController> logger) : base(logger)
    {
    }

    /// <summary>
    /// Verifica el estado de la API.
    /// </summary>
    /// <returns>Estado de la API.</returns>
    /// <response code="200">La API está funcionando correctamente.</response>
    [HttpGet("status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult GetStatus()
    {
        _logger.LogInformation("Verificación de estado de la API solicitada. CorrelationId: {CorrelationId}", CorrelationId);

        var response = ApiResponse<object>.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        }, "API funcionando correctamente", CorrelationId);

        return Ok(response);
    }

    /// <summary>
    /// Obtiene información de la versión de la API.
    /// </summary>
    /// <returns>Información de versión.</returns>
    /// <response code="200">Información de versión retornada exitosamente.</response>
    [HttpGet("version")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult GetVersion()
    {
        var response = ApiResponse<object>.Ok(new
        {
            Version = "1.0.0",
            BuildDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Framework = ".NET 8"
        }, correlationId: CorrelationId);

        return Ok(response);
    }
}
