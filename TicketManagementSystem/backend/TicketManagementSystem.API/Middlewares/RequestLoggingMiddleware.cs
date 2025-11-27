using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TicketManagementSystem.API.Middlewares
{
    /// <summary>
    /// Middleware for logging HTTP requests with correlation ID and performance metrics
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Generate correlation ID if not present
            if (!context.Request.Headers.ContainsKey("X-Request-ID"))
            {
                context.Request.Headers["X-Request-ID"] = Guid.NewGuid().ToString();
            }
            var correlationId = context.Request.Headers["X-Request-ID"].ToString();

            // Skip logging for static routes
            var path = context.Request.Path.Value;
            if (path != null && (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images")))
            {
                await _next(context);
                return;
            }

            // Add headers before response starts
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey("X-Request-ID"))
                {
                    context.Response.Headers["X-Request-ID"] = correlationId;
                }
                if (!context.Response.Headers.ContainsKey("X-Response-Time"))
                {
                    context.Response.Headers["X-Response-Time"] = stopwatch.ElapsedMilliseconds.ToString();
                }
                return Task.CompletedTask;
            });

            try
            {
                await _next(context);

                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;
                var statusCode = context.Response.StatusCode;
                var method = context.Request.Method;
                var userAgent = SanitizeHeader(context.Request.Headers["User-Agent"].ToString());
                var sanitizedHeaders = SanitizeHeaders(context.Request.Headers);

                if (statusCode >= 500)
                {
                    _logger.LogError("Request failed: {Method} {Path} - Status: {StatusCode} - Elapsed: {Elapsed}ms - UserAgent: {UserAgent} - CorrelationId: {CorrelationId}",
                        method, path, statusCode, elapsed, userAgent, correlationId);
                }
                else if (elapsed > 5000)
                {
                    _logger.LogWarning("Slow request: {Method} {Path} - Status: {StatusCode} - Elapsed: {Elapsed}ms - UserAgent: {UserAgent} - CorrelationId: {CorrelationId}",
                        method, path, statusCode, elapsed, userAgent, correlationId);
                }
                else
                {
                _logger.LogInformation("Request: {Method} {Path} - Status: {StatusCode} - Elapsed: {Elapsed}ms - UserAgent: {UserAgent} - CorrelationId: {CorrelationId} - Headers: {@Headers}",
                    method, path, statusCode, elapsed, userAgent, correlationId, sanitizedHeaders);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Unhandled exception in request: {Method} {Path} - CorrelationId: {CorrelationId}",
                    context.Request.Method, path, correlationId);
                throw;
            }
        }

        /// <summary>
        /// Sanitize sensitive header values
        /// </summary>
        private static string SanitizeHeader(string headerValue)
        {
            if (string.IsNullOrEmpty(headerValue))
                return headerValue;

            // Remove or mask sensitive information
            // For example, if it contains tokens or passwords
            if (headerValue.Length > 50)
            {
                return headerValue.Substring(0, 50) + "...[TRUNCATED]";
            }

            return headerValue;
        }

        /// <summary>
        /// Sanitize request headers to remove sensitive information
        /// </summary>
        private static Dictionary<string, string> SanitizeHeaders(IHeaderDictionary headers)
        {
            var sanitized = new Dictionary<string, string>();
            var sensitiveHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Authorization",
                "X-API-Key",
                "X-Auth-Token",
                "Cookie",
                "Set-Cookie",
                "Proxy-Authorization"
            };

            foreach (var header in headers)
            {
                if (sensitiveHeaders.Contains(header.Key))
                {
                    sanitized[header.Key] = "***REDACTED***";
                }
                else
                {
                    sanitized[header.Key] = SanitizeHeader(header.Value.ToString());
                }
            }

            return sanitized;
        }
    }
}