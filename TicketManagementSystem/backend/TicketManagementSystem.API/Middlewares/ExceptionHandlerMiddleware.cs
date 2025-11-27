using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.Middlewares
{
    /// <summary>
    /// Middleware global para manejo de excepciones no controladas
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var problemDetails = CreateProblemDetails(context, exception, statusCode);

            _logger.LogError(exception, "Unhandled exception occurred. Request: {Method} {Path}, Status: {StatusCode}",
                context.Request.Method, context.Request.Path, statusCode);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                InvalidOperationException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private static object CreateProblemDetails(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            var stackTrace = context.RequestServices.GetService(typeof(IHostEnvironment)) is IHostEnvironment env && env.IsDevelopment()
                ? exception.StackTrace
                : null;

            var problemDetails = new
            {
                type = $"https://httpstatuses.com/{(int)statusCode}",
                title = GetTitle(statusCode),
                status = (int)statusCode,
                detail = exception.Message,
                instance = context.Request.Path,
                traceId = context.TraceIdentifier,
                stackTrace = stackTrace
            };

            return problemDetails;
        }

        private static string GetTitle(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.NotFound => "Not Found",
                HttpStatusCode.Forbidden => "Forbidden",
                HttpStatusCode.Conflict => "Conflict",
                _ => "Internal Server Error"
            };
        }
    }

    /// <summary>
    /// Extension methods for registering the exception handler middleware
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}