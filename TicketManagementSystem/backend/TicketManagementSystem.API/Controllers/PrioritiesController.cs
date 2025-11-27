using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de prioridades del sistema.
    /// Proporciona endpoints CRUD para administrar las prioridades de tickets.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requiere autenticación para todas las operaciones
    public class PrioritiesController : ControllerBase
    {
        private readonly IPriorityService _priorityService;
        private readonly IMapper _mapper;
        private readonly ILogger<PrioritiesController> _logger;
        private readonly IValidator<GetPrioritiesQueryParameters> _validator;

        /// <summary>
        /// Constructor del controlador de prioridades.
        /// </summary>
        /// <param name="priorityService">Servicio de prioridades.</param>
        /// <param name="mapper">Instancia de AutoMapper.</param>
        /// <param name="logger">Logger para registro de operaciones.</param>
        /// <param name="validator">Validador para parámetros de consulta.</param>
        public PrioritiesController(
            IPriorityService priorityService,
            IMapper mapper,
            ILogger<PrioritiesController> logger,
            IValidator<GetPrioritiesQueryParameters> validator)
        {
            _priorityService = priorityService;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Obtiene una lista paginada de prioridades.
        /// </summary>
        /// <param name="page">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Elementos por página (por defecto 10).</param>
        /// <param name="name">Filtrar por nombre.</param>
        /// <param name="isActive">Filtrar por estado activo.</param>
        /// <param name="sortBy">Ordenar por: name, level, createdAt.</param>
        /// <param name="sortOrder">Orden: asc o desc.</param>
        /// <returns>Lista paginada de prioridades.</returns>
        /// <response code="200">Retorna la lista paginada de prioridades.</response>
        /// <response code="400">Parámetros de consulta inválidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PriorityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<PriorityDto>>> GetPriorities(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] string? sortBy = "level",
            [FromQuery] string? sortOrder = "asc")
        {
            var parameters = new GetPrioritiesQueryParameters
            {
                Page = page,
                PageSize = pageSize,
                Name = name,
                IsActive = isActive,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            var validationResult = await _validator.ValidateAsync(parameters);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Parámetros de consulta inválidos: {Errors}",
                    validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ProblemDetails
                {
                    Title = "Parámetros inválidos",
                    Detail = "Uno o más parámetros de consulta son inválidos",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            try
            {
                var result = await _priorityService.GetPrioritiesAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener prioridades");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al obtener las prioridades",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Obtiene una prioridad por su ID.
        /// </summary>
        /// <param name="id">ID de la prioridad.</param>
        /// <returns>La prioridad solicitada.</returns>
        /// <response code="200">Retorna la prioridad.</response>
        /// <response code="404">Prioridad no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PriorityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PriorityDto>> GetPriority(int id)
        {
            try
            {
                var priority = await _priorityService.GetPriorityByIdAsync(id);
                if (priority == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Prioridad no encontrada",
                        Detail = $"No se encontró una prioridad con ID {id}",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Ok(priority);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener prioridad con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al obtener la prioridad",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Crea una nueva prioridad.
        /// Requiere rol de Admin.
        /// </summary>
        /// <param name="dto">Datos de la nueva prioridad.</param>
        /// <returns>La prioridad creada.</returns>
        /// <response code="201">Prioridad creada exitosamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="403">No autorizado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PriorityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PriorityDto>> CreatePriority([FromBody] CreatePriorityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = "Los datos proporcionados no son válidos",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) }
                });
            }

            try
            {
                var priority = await _priorityService.CreatePriorityAsync(dto);
                return CreatedAtAction(nameof(GetPriority), new { id = priority.Id }, priority);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Intento de crear prioridad con datos inválidos");
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear prioridad");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al crear la prioridad",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Actualiza una prioridad existente.
        /// Requiere rol de Admin.
        /// </summary>
        /// <param name="id">ID de la prioridad a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>La prioridad actualizada.</returns>
        /// <response code="200">Prioridad actualizada exitosamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="403">No autorizado.</response>
        /// <response code="404">Prioridad no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PriorityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PriorityDto>> UpdatePriority(int id, [FromBody] UpdatePriorityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = "Los datos proporcionados no son válidos",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) }
                });
            }

            try
            {
                var priority = await _priorityService.UpdatePriorityAsync(id, dto);
                return Ok(priority);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Intento de actualizar prioridad inexistente con ID {Id}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Prioridad no encontrada",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Intento de actualizar prioridad con datos inválidos");
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar prioridad con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al actualizar la prioridad",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Elimina una prioridad.
        /// Requiere rol de Admin.
        /// </summary>
        /// <param name="id">ID de la prioridad a eliminar.</param>
        /// <returns>No content si se eliminó exitosamente.</returns>
        /// <response code="204">Prioridad eliminada exitosamente.</response>
        /// <response code="403">No autorizado.</response>
        /// <response code="404">Prioridad no encontrada.</response>
        /// <response code="409">Conflicto - prioridad en uso.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePriority(int id)
        {
            try
            {
                await _priorityService.DeletePriorityAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Intento de eliminar prioridad inexistente con ID {Id}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Prioridad no encontrada",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Intento de eliminar prioridad en uso con ID {Id}", id);
                return Conflict(new ProblemDetails
                {
                    Title = "Conflicto",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar prioridad con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al eliminar la prioridad",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Activa o desactiva una prioridad.
        /// Requiere rol de Admin.
        /// </summary>
        /// <param name="id">ID de la prioridad.</param>
        /// <param name="isActive">Estado deseado.</param>
        /// <returns>No content si se cambió exitosamente.</returns>
        /// <response code="204">Estado cambiado exitosamente.</response>
        /// <response code="403">No autorizado.</response>
        /// <response code="404">Prioridad no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPatch("{id}/toggle-active")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TogglePriorityActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                await _priorityService.TogglePriorityActiveAsync(id, isActive);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Intento de cambiar estado de prioridad inexistente con ID {Id}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Prioridad no encontrada",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de prioridad con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al cambiar el estado de la prioridad",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }

        /// <summary>
        /// Obtiene todas las prioridades activas.
        /// </summary>
        /// <returns>Lista de prioridades activas.</returns>
        /// <response code="200">Retorna las prioridades activas.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(List<PriorityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PriorityDto>>> GetActivePriorities()
        {
            try
            {
                var priorities = await _priorityService.GetActivePrioritiesAsync();
                return Ok(priorities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener prioridades activas");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Error interno",
                        Detail = "Ocurrió un error al obtener las prioridades activas",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }
    }
}