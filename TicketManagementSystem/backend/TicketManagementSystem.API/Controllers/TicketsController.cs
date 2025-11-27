using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.CQRS.Commands;
using TicketManagementSystem.API.CQRS.Queries;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Services;
using TicketManagementSystem.API.Validators;

namespace TicketManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing tickets
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TicketsController : BaseApiController
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly ITicketMetricsService _metricsService;
        private readonly ITicketAuthorizationService _ticketAuthorizationService;
        private readonly IMapper _mapper;
        private readonly IValidator<GetTicketsQueryParameters> _validator;
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for TicketsController
        /// </summary>
        /// <param name="ticketService">Ticket service for business logic</param>
        /// <param name="userService">User service for access control</param>
        /// <param name="metricsService">Ticket metrics service for statistics</param>
        /// <param name="ticketAuthorizationService">Ticket authorization service</param>
        /// <param name="mapper">AutoMapper instance for object mapping</param>
        /// <param name="logger">Logger for structured logging</param>
        /// <param name="validator">Validator for query parameters</param>
        /// <param name="mediator">Mediator for CQRS</param>
        public TicketsController(
            ITicketService ticketService,
            IUserService userService,
            ITicketMetricsService metricsService,
            ITicketAuthorizationService ticketAuthorizationService,
            IMapper mapper,
            ILogger<TicketsController> logger,
            IValidator<GetTicketsQueryParameters> validator,
            IMediator mediator)
            : base(logger)
        {
            _ticketService = ticketService;
            _userService = userService;
            _metricsService = metricsService;
            _ticketAuthorizationService = ticketAuthorizationService;
            _mapper = mapper;
            _validator = validator;
            _mediator = mediator;
        }

        /// <summary>
        /// Get paginated list of tickets with optional filters and sorting
        /// </summary>
        /// <param name="page">Page number (default 1, minimum 1)</param>
        /// <param name="pageSize">Items per page (default 10, minimum 1, maximum 100)</param>
        /// <param name="status">Filter by status: Open, InProgress, Resolved, Closed</param>
        /// <param name="priorityId">Filter by priority ID</param>
        /// <param name="assignedTo">Filter by assigned user ID</param>
        /// <param name="search">Search in title and description</param>
        /// <param name="sortBy">Sort by: createdAt, updatedAt, priority (default createdAt)</param>
        /// <param name="sortOrder">Sort order: asc or desc (default desc)</param>
        /// <returns>Paged response containing tickets</returns>
        /// <response code="200">Returns paginated list of tickets</response>
        /// <response code="400">Invalid query parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<TicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<TicketDto>>> GetTickets(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] int? priorityId = null,
            [FromQuery] int? assignedTo = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] string? sortOrder = "desc")
        {
            var parameters = new GetTicketsQueryParameters
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                PriorityId = priorityId,
                AssignedTo = assignedTo,
                Search = search,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            var validationResult = await _validator.ValidateAsync(parameters);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid query parameters: {Errors}", validationResult.Errors);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "One or more query parameters are invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            var result = await _mediator.Send(new GetTicketsQuery(parameters));
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to retrieve tickets: {Error}", result.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, CreateProblemDetails("Internal Server Error", result.Error, StatusCodes.Status500InternalServerError));
            }

            _logger.LogInformation("Successfully retrieved {Count} tickets for page {Page}", result.Value!.Items.Count, result.Value!.Page);
            return Ok(result.Value);
        }

        /// <summary>
        /// Get a specific ticket by ID
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <returns>Ticket details</returns>
        /// <response code="200">Returns the ticket</response>
        /// <response code="400">Invalid ticket ID</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var result = await _mediator.Send(new GetTicketByIdQuery(id));
            if (!result.IsSuccess)
            {
                if (result.ErrorCode == "NotFound")
                {
                    _logger.LogWarning("Ticket {Id} not found", id);
                    return NotFound(CreateProblemDetails("Not Found", result.Error, StatusCodes.Status404NotFound));
                }
                _logger.LogError("Failed to retrieve ticket {Id}: {Error}", id, result.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, CreateProblemDetails("Internal Server Error", result.Error, StatusCodes.Status500InternalServerError));
            }

            var ticket = result.Value;
            var currentUserId = GetCurrentUserId();
            var accessResult = await _ticketAuthorizationService.CanAccessTicketAsync(id, currentUserId);
            if (!accessResult.IsSuccess)
            {
                _logger.LogWarning("User {UserId} attempted to access unauthorized ticket {TicketId}", currentUserId, id);
                return Forbid();
            }

            _logger.LogInformation("Successfully retrieved ticket {Id} for user {UserId}", id, currentUserId);
            return Ok(ticket);
        }

        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <param name="dto">Ticket creation data</param>
        /// <returns>Created ticket</returns>
        /// <response code="201">Ticket created successfully</response>
        /// <response code="400">Invalid ticket data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userId = GetCurrentUserId();
            var createResult = await _mediator.Send(new CreateTicketCommand(dto, userId));
            if (!createResult.IsSuccess)
            {
                _logger.LogError("Failed to create ticket: {Error}", createResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, CreateProblemDetails("Internal Server Error", createResult.Error, StatusCodes.Status500InternalServerError));
            }

            var ticket = createResult.Value;
            var ticketDto = _mapper.Map<TicketDto>(ticket);

            _logger.LogInformation("Ticket {Id} created by user {UserId}", ticket!.Id, userId);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticketDto);
        }

        /// <summary>
        /// Update an existing ticket
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="dto">Ticket update data</param>
        /// <returns>Updated ticket</returns>
        /// <response code="200">Ticket updated successfully</response>
        /// <response code="400">Invalid ticket data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketDto>> UpdateTicket(int id, [FromBody] UpdateTicketDto dto)
        {
            // Note: UpdateTicketDto validation would need its own validator
            // For now, using basic validation

            try
            {
                // Check object-level access control before updating
                var getResult = await _ticketService.GetByIdAsync(id, HttpContext.RequestAborted);
                if (!getResult.IsSuccess)
                {
                    if (getResult.ErrorCode == "NotFound")
                    {
                        _logger.LogWarning("Ticket {Id} not found", id);
                        return NotFound(new ProblemDetails
                        {
                            Title = "Ticket Not Found",
                            Detail = getResult.Error,
                            Status = StatusCodes.Status404NotFound
                        });
                    }
                    _logger.LogError("Failed to retrieve ticket {Id}: {Error}", id, getResult.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = getResult.Error,
                        Status = StatusCodes.Status500InternalServerError
                    });
                }

                var existingTicket = getResult.Value!;
                var currentUserId = GetCurrentUserId();
                var currentUser = await _userService.GetUserByIdAsync(currentUserId);
                
                if (existingTicket.CreatedById != currentUserId && 
                    existingTicket.AssignedToId != currentUserId && 
                    currentUser.Role != Constants.UserRoles.Admin && 
                    currentUser.Role != Constants.UserRoles.Agent)
                {
                    _logger.LogWarning("User {UserId} attempted to update unauthorized ticket {TicketId}", currentUserId, id);
                    return Forbid();
                }

                var updateResult = await _mediator.Send(new UpdateTicketCommand(id, dto));
                if (!updateResult.IsSuccess)
                {
                    _logger.LogError("Failed to update ticket {Id}: {Error}", id, updateResult.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = updateResult.Error,
                        Status = StatusCodes.Status500InternalServerError
                    });
                }

                var ticket = updateResult.Value;
                var ticketDto = _mapper.Map<TicketDto>(ticket);

                _logger.LogInformation("Ticket {Id} updated by user {UserId}", id, currentUserId);
                return Ok(ticketDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Ticket {Id} not found", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Ticket Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized ticket update attempt for ticket {Id}", id);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Delete a ticket (soft delete)
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <response code="204">Ticket deleted successfully</response>
        /// <response code="400">Invalid ticket ID</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                await _mediator.Send(new DeleteTicketCommand(id));
                _logger.LogInformation("Ticket {Id} deleted", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Ticket {Id} not found", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Ticket Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized ticket deletion attempt for ticket {Id}", id);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get tickets for the current user (created by or assigned to)
        /// </summary>
        /// <returns>List of user's tickets</returns>
        /// <response code="200">Returns user's tickets</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("my-tickets")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(List<TicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TicketDto>>> GetMyTickets()
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _mediator.Send(new GetUserTicketsQuery(userId));
                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to retrieve tickets for user {UserId}: {Error}", userId, result.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = result.Error,
                        Status = StatusCodes.Status500InternalServerError
                    });
                }

                var tickets = result.Value!;
                _logger.LogInformation("Retrieved {Count} tickets for user {UserId}", tickets.Count, userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets for current user");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get ticket statistics
        /// </summary>
        /// <param name="from">Start date (optional)</param>
        /// <param name="to">End date (optional)</param>
        /// <returns>Ticket statistics</returns>
        /// <response code="200">Returns ticket statistics</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("stats")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(TicketMetrics), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketMetrics>> GetTicketStats(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var fromDate = from ?? DateTime.UtcNow.AddDays(-30);
                var toDate = to ?? DateTime.UtcNow;

                var metrics = await _metricsService.GetMetricsAsync(userId, fromDate, toDate);

                _logger.LogInformation("Retrieved ticket stats for user {UserId}", userId);
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket statistics");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get ticket history with pagination and optional filters
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="filter">Optional filter and pagination parameters</param>
        /// <returns>Paginated ticket history entries with enriched user information</returns>
        /// <response code="200">Returns paginated ticket history</response>
        /// <response code="400">Invalid ticket ID or filter parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">User does not have access to this ticket</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/tickets/1/history?page=1&amp;pageSize=20&amp;fromDate=2025-01-01
        /// 
        /// The response includes:
        /// - Paginated history entries with user names resolved
        /// - Each entry contains a list of specific changes detected
        /// - IsCreation flag indicates if it's the ticket creation entry
        /// </remarks>
        [HttpGet("{id}/history")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(PagedResponse<TicketHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<TicketHistoryDto>>> GetTicketHistory(
            int id, 
            [FromQuery] TicketHistoryFilterDto? filter)
        {
            try
            {
                // Validate ticket ID
                if (id <= 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Invalid Ticket ID",
                        Detail = "Ticket ID must be greater than 0",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                // Check if ticket exists and user has access
                var ticketResult = await _ticketService.GetByIdAsync(id, HttpContext.RequestAborted);
                if (!ticketResult.IsSuccess)
                {
                    if (ticketResult.ErrorCode == "NotFound")
                    {
                        _logger.LogWarning("Ticket {Id} not found", id);
                        return NotFound(new ProblemDetails
                        {
                            Title = "Ticket Not Found",
                            Detail = $"Ticket with ID {id} was not found",
                            Status = StatusCodes.Status404NotFound
                        });
                    }
                    _logger.LogError("Failed to retrieve ticket {Id}: {Error}", id, ticketResult.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = ticketResult.Error,
                        Status = StatusCodes.Status500InternalServerError
                    });
                }

                var ticket = ticketResult.Value!;

                // Object-level access control
                var currentUserId = GetCurrentUserId();
                var currentUser = await _userService.GetUserByIdAsync(currentUserId);

                if (ticket.CreatedById != currentUserId &&
                    ticket.AssignedToId != currentUserId &&
                    currentUser.Role != Constants.UserRoles.Admin &&
                    currentUser.Role != Constants.UserRoles.Agent)
                {
                    _logger.LogWarning("User {UserId} attempted to access history of unauthorized ticket {TicketId}", currentUserId, id);
                    return Forbid();
                }

                // Get paginated history with enriched data
                var historyResult = await _ticketService.GetTicketHistoryAsync(id, filter, HttpContext.RequestAborted);
                if (!historyResult.IsSuccess)
                {
                    _logger.LogError("Failed to retrieve history for ticket {TicketId}: {Error}", id, historyResult.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = historyResult.Error,
                        Status = StatusCodes.Status500InternalServerError
                    });
                }

                var response = historyResult.Value!;
                _logger.LogInformation(
                    "Retrieved {Count} of {Total} history entries for ticket {TicketId} (page {Page})", 
                    response.Items.Count, 
                    response.TotalItems, 
                    id,
                    response.Page);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving history for ticket {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}