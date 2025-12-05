using BookingSystemAPI.Api.Common.Responses;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador para gestión de reservas.
/// Requiere autenticación JWT para todos los endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // OWASP A01:2021 - Broken Access Control
[EnableRateLimiting("general")]
[Tags("📅 Reservas")]
public class BookingsController : BaseApiController
{
    private readonly IBookingService _bookingService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de reservas.
    /// </summary>
    /// <param name="bookingService">Servicio de reservas.</param>
    /// <param name="logger">Logger.</param>
    public BookingsController(
        IBookingService bookingService,
        ILogger<BookingsController> logger) : base(logger)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Obtiene todas las reservas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas.</returns>
    /// <response code="200">Lista de reservas obtenida exitosamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetAllAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<BookingDto>>.Ok(
            result.Value!,
            correlationId: CorrelationId));
    }

    /// <summary>
    /// Obtiene las reservas del usuario actual.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas del usuario.</returns>
    /// <response code="200">Lista de reservas del usuario obtenida exitosamente.</response>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBookings(CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetByCurrentUserAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<BookingDto>>.Ok(
            result.Value!,
            correlationId: CorrelationId));
    }

    /// <summary>
    /// Obtiene una reserva por su ID.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva encontrada.</returns>
    /// <response code="200">Reserva encontrada.</response>
    /// <response code="404">Reserva no encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return Ok(ApiResponse<BookingDto>.Ok(
            result.Value!,
            correlationId: CorrelationId));
    }

    /// <summary>
    /// Crea una nueva reserva.
    /// </summary>
    /// <param name="dto">Datos de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva creada.</returns>
    /// <response code="201">Reserva creada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="409">Conflicto de horario.</response>
    /// <response code="422">Error de regla de negocio.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Solicitud de creación de reserva para sala {RoomId}", dto.RoomId);

        var result = await _bookingService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        var response = ApiResponse<BookingDto>.Ok(
            result.Value!,
            "Reserva creada exitosamente",
            CorrelationId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            response);
    }

    /// <summary>
    /// Actualiza una reserva existente.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="dto">Nuevos datos de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva actualizada.</returns>
    /// <response code="200">Reserva actualizada exitosamente.</response>
    /// <response code="404">Reserva no encontrada.</response>
    /// <response code="409">Conflicto de horario.</response>
    /// <response code="422">Error de regla de negocio.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto, CancellationToken cancellationToken)
    {
        var result = await _bookingService.UpdateAsync(id, dto, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return Ok(ApiResponse<BookingDto>.Ok(
            result.Value!,
            "Reserva actualizada exitosamente",
            CorrelationId));
    }

    /// <summary>
    /// Cancela una reserva.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    /// <response code="204">Reserva cancelada exitosamente.</response>
    /// <response code="404">Reserva no encontrada.</response>
    /// <response code="422">No se puede cancelar la reserva.</response>
    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.CancelAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return NoContent();
    }

    /// <summary>
    /// Elimina una reserva (soft delete).
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    /// <response code="204">Reserva eliminada exitosamente.</response>
    /// <response code="404">Reserva no encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return ToErrorResult(result.Error!);
        }

        return NoContent();
    }

    /// <summary>
    /// Obtiene las reservas de una sala en un rango de fechas.
    /// </summary>
    /// <param name="roomId">ID de la sala.</param>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas.</returns>
    [HttpGet("room/{roomId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoom(
        int roomId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetByRoomAndDateRangeAsync(
            roomId, startDate, endDate, cancellationToken);

        return Ok(ApiResponse<IEnumerable<BookingDto>>.Ok(
            result.Value!,
            correlationId: CorrelationId));
    }

    /// <summary>
    /// Convierte un Error del Result Pattern a una respuesta HTTP apropiada.
    /// </summary>
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
            Title = GetTitleForError(error.Code),
            Detail = error.Message,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["correlationId"] = CorrelationId;
        problemDetails.Extensions["errorCode"] = error.Code;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        return StatusCode(statusCode, problemDetails);
    }

    private static string GetTitleForError(string errorCode) => errorCode switch
    {
        "NotFound" => "Recurso no encontrado",
        "Conflict" => "Conflicto",
        "Validation" => "Error de validación",
        _ => "Error de negocio"
    };
}
