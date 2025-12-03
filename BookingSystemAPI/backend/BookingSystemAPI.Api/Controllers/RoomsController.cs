using BookingSystemAPI.Api.Common.Responses;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Rooms;
using BookingSystemAPI.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador para gestión de salas.
/// Requiere autenticación JWT para todos los endpoints.
/// </summary>
[Route("api/[controller]")]
[Authorize] // OWASP A01:2021 - Broken Access Control
[EnableRateLimiting("general")]
public class RoomsController : BaseApiController
{
    private readonly IRoomService _roomService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de salas.
    /// </summary>
    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger) 
        : base(logger)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Obtiene todas las salas.
    /// </summary>
    /// <response code="200">Lista de salas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _roomService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<RoomDto>>.Ok(result.Value!, correlationId: CorrelationId));
    }

    /// <summary>
    /// Obtiene una sala por ID.
    /// </summary>
    /// <param name="id">ID de la sala.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="200">Sala encontrada.</response>
    /// <response code="404">Sala no encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _roomService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return Ok(ApiResponse<RoomDto>.Ok(result.Value!, correlationId: CorrelationId));
    }

    /// <summary>
    /// Crea una nueva sala.
    /// </summary>
    /// <param name="dto">Datos de la sala.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="201">Sala creada.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="409">Ya existe una sala con ese nombre.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creando sala: {RoomName}", dto.Name);

        var result = await _roomService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            ApiResponse<RoomDto>.Ok(result.Value!, "Sala creada exitosamente", CorrelationId));
    }

    /// <summary>
    /// Actualiza una sala existente.
    /// </summary>
    /// <param name="id">ID de la sala.</param>
    /// <param name="dto">Nuevos datos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="200">Sala actualizada.</response>
    /// <response code="404">Sala no encontrada.</response>
    /// <response code="409">Ya existe otra sala con ese nombre.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto, CancellationToken cancellationToken)
    {
        var result = await _roomService.UpdateAsync(id, dto, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return Ok(ApiResponse<RoomDto>.Ok(result.Value!, "Sala actualizada exitosamente", CorrelationId));
    }

    /// <summary>
    /// Elimina una sala (soft delete).
    /// </summary>
    /// <param name="id">ID de la sala.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <response code="204">Sala eliminada.</response>
    /// <response code="404">Sala no encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _roomService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return NoContent();
    }

    /// <summary>
    /// Pone una sala en mantenimiento.
    /// </summary>
    /// <param name="id">ID de la sala.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpPost("{id:int}/maintenance")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetMaintenance(int id, CancellationToken cancellationToken)
    {
        var result = await _roomService.SetMaintenanceAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return NoContent();
    }

    /// <summary>
    /// Pone una sala disponible.
    /// </summary>
    /// <param name="id">ID de la sala.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpPost("{id:int}/available")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetAvailable(int id, CancellationToken cancellationToken)
    {
        var result = await _roomService.SetAvailableAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return NoContent();
    }

    private IActionResult ToErrorResult(Error error)
    {
        var statusCode = error.Code switch
        {
            "NotFound" => StatusCodes.Status404NotFound,
            "Conflict" => StatusCodes.Status409Conflict,
            "Validation" => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status422UnprocessableEntity
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code == "NotFound" ? "Recurso no encontrado" :
                    error.Code == "Conflict" ? "Conflicto" : "Error",
            Detail = error.Message,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["correlationId"] = CorrelationId;

        return StatusCode(statusCode, problemDetails);
    }
}
