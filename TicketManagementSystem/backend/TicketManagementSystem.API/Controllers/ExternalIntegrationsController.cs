using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.API.Controllers;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Services;

/// <summary>
/// Controlador para integraciones externas.
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class ExternalIntegrationsController : BaseApiController
{
    private readonly ITavilyService _tavilyService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ExternalIntegrationsController(ILogger<ExternalIntegrationsController> logger, ITavilyService tavilyService)
        : base(logger)
    {
        _tavilyService = tavilyService;
    }

    /// <summary>
    /// Realiza una búsqueda web utilizando Tavily para obtener información sobre dependencias e integraciones.
    /// </summary>
    /// <param name="request">El objeto de solicitud que contiene la consulta de búsqueda.</param>
    /// <returns>Los resultados de la búsqueda.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(TavilyResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Search([FromBody] SearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("La consulta no puede estar vacía.");
        }

        try
        {
            var result = await _tavilyService.SearchAsync(request.Query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al realizar búsqueda con query: {Query}", request.Query);
            return StatusCode(500, "Error interno del servidor.");
        }
    }
}

/// <summary>
/// Solicitud de búsqueda.
/// </summary>
public class SearchRequest
{
    /// <summary>
    /// La consulta de búsqueda.
    /// </summary>
    public string Query { get; set; } = string.Empty;
}