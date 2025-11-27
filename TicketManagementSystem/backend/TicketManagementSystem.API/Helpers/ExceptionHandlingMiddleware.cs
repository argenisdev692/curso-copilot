using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Middleware for handling exceptions globally
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleUnauthorizedExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");

            var correlationId = Guid.NewGuid().ToString();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Validation Error",
                status = (int)HttpStatusCode.BadRequest,
                detail = "One or more validation errors occurred",
                instance = context.Request.Path,
                correlationId = correlationId,
                errors = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                })
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");

            var correlationId = Guid.NewGuid().ToString();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                title = "Not Found",
                status = (int)HttpStatusCode.NotFound,
                detail = ex.Message,
                instance = context.Request.Path,
                correlationId = correlationId
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt");

            var correlationId = Guid.NewGuid().ToString();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                title = "Unauthorized",
                status = (int)HttpStatusCode.Unauthorized,
                detail = ex.Message,
                instance = context.Request.Path,
                correlationId = correlationId
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");

            var correlationId = Guid.NewGuid().ToString();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "Internal Server Error",
                status = (int)HttpStatusCode.InternalServerError,
                detail = "An unexpected error occurred. Please try again later.",
                instance = context.Request.Path,
                correlationId = correlationId
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}