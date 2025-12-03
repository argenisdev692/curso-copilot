using Microsoft.AspNetCore.Mvc;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador base con funcionalidades comunes para todos los controladores API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Logger para el controlador.
    /// </summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Inicializa una nueva instancia del controlador base.
    /// </summary>
    /// <param name="logger">Logger para registro de operaciones.</param>
    protected BaseApiController(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el CorrelationId del request actual.
    /// </summary>
    protected string CorrelationId =>
        HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
}
