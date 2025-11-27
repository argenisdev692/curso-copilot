using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Controllers
{
    /// <summary>
    /// Base controller with common functionality
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseApiController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a standardized ProblemDetails response
        /// </summary>
        protected ProblemDetails CreateProblemDetails(string title, string detail, int statusCode, IEnumerable<string>? errors = null)
        {
            var problem = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = HttpContext.Request.Path
            };

            if (errors != null && errors.Any())
            {
                problem.Extensions["errors"] = errors;
            }

            return problem;
        }

        /// <summary>
        /// Logs validation errors and returns BadRequest
        /// </summary>
        protected BadRequestObjectResult HandleValidationErrors(IEnumerable<string> errors, string context)
        {
            _logger.LogWarning("Validation errors in {Context}: {Errors}", context, string.Join(", ", errors));
            return BadRequest(CreateProblemDetails("Validation Error", "One or more validation errors occurred", StatusCodes.Status400BadRequest, errors));
        }

        /// <summary>
        /// Logs exception and returns InternalServerError
        /// </summary>
        protected ObjectResult HandleException(Exception ex, string operation, params object[] args)
        {
            _logger.LogError(ex, "Error during {Operation}", operation);
            return StatusCode(StatusCodes.Status500InternalServerError,
                CreateProblemDetails("Internal Server Error", "An unexpected error occurred", StatusCodes.Status500InternalServerError));
        }

        /// <summary>
        /// Gets the current authenticated user ID from claims
        /// </summary>
        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("Invalid user token");
        }
    }
}