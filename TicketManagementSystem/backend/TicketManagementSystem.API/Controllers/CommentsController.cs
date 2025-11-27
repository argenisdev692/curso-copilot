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
    /// Controller for managing comments
    /// </summary>
    [ApiController]
    [Route("api/tickets/{ticketId}/[controller]")]
    [Produces("application/json")]
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCommentDto> _validator;

        /// <summary>
        /// Constructor for CommentsController
        /// </summary>
        /// <param name="commentService">Comment service for business logic</param>
        /// <param name="mapper">AutoMapper instance for object mapping</param>
        /// <param name="logger">Logger for structured logging</param>
        /// <param name="validator">Validator for comment DTOs</param>
        public CommentsController(
            ICommentService commentService,
            IMapper mapper,
            ILogger<CommentsController> logger,
            IValidator<CreateCommentDto> validator)
            : base(logger)
        {
            _commentService = commentService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Get all comments for a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <returns>List of comments</returns>
        /// <response code="200">Returns list of comments</response>
        /// <response code="400">Invalid ticket ID</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CommentDto>>> GetComments(int ticketId)
        {
            try
            {
                var comments = await _commentService.GetByTicketIdAsync(ticketId, HttpContext.RequestAborted);
                _logger.LogInformation("Successfully retrieved {Count} comments for ticket {TicketId}", comments.Count, ticketId);
                return Ok(comments);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Ticket {TicketId} not found", ticketId);
                return NotFound(new ProblemDetails
                {
                    Title = "Ticket Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving comments for ticket {TicketId}", ticketId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Add a new comment to a ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="dto">Comment creation data</param>
        /// <returns>Created comment</returns>
        /// <response code="201">Comment created successfully</response>
        /// <response code="400">Invalid comment data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Ticket not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommentDto>> CreateComment(int ticketId, [FromBody] CreateCommentDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid comment data: {Errors}", validationResult.Errors);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "One or more fields are invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            try
            {
                var userId = GetCurrentUserId();
                var comment = await _commentService.AddAsync(ticketId, dto, userId, HttpContext.RequestAborted);
                _logger.LogInformation("Comment created for ticket {TicketId} by user {UserId}", ticketId, userId);
                return CreatedAtAction(nameof(GetComments), new { ticketId }, comment);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Ticket {TicketId} not found", ticketId);
                return NotFound(new ProblemDetails
                {
                    Title = "Ticket Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access for ticket {TicketId}", ticketId);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment for ticket {TicketId}", ticketId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Update an existing comment
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="commentId">Comment ID</param>
        /// <param name="dto">Comment update data</param>
        /// <returns>Updated comment</returns>
        /// <response code="200">Comment updated successfully</response>
        /// <response code="400">Invalid comment data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Comment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{commentId}")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommentDto>> UpdateComment(int ticketId, int commentId, [FromBody] UpdateCommentDto dto)
        {
            var validationResult = await _validator.ValidateAsync(new CreateCommentDto { Content = dto.Content });
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid comment data: {Errors}", validationResult.Errors);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "One or more fields are invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            try
            {
                var userId = GetCurrentUserId();
                var comment = await _commentService.UpdateAsync(commentId, dto, userId, HttpContext.RequestAborted);
                _logger.LogInformation("Comment {CommentId} updated by user {UserId}", commentId, userId);
                return Ok(comment);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Comment {CommentId} not found", commentId);
                return NotFound(new ProblemDetails
                {
                    Title = "Comment Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access for comment {CommentId}", commentId);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment {CommentId}", commentId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="commentId">Comment ID</param>
        /// <response code="204">Comment deleted successfully</response>
        /// <response code="400">Invalid comment ID</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Comment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{commentId}")]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int ticketId, int commentId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _commentService.DeleteAsync(commentId, userId, HttpContext.RequestAborted);
                _logger.LogInformation("Comment {CommentId} deleted by user {UserId}", commentId, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Comment {CommentId} not found", commentId);
                return NotFound(new ProblemDetails
                {
                    Title = "Comment Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access for comment {CommentId}", commentId);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment {CommentId}", commentId);
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