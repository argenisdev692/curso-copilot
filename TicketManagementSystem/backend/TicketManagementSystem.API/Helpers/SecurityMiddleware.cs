using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Middleware for additional security checks and rate limiting
    /// </summary>
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityMiddleware> _logger;
        private readonly IMemoryCache _cache;

        public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger, IMemoryCache cache)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Rate limiting check
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var correlationId = context.Request.Headers["X-Request-ID"].ToString() ?? Guid.NewGuid().ToString();
            var key = $"{ip}:{DateTime.UtcNow.ToString("yyyyMMddHHmm")}";

            var count = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return 0;
            });

            count++;
            _cache.Set(key, count);

            if (count > 100)
            {
                _logger.LogWarning("Rate limit exceeded for IP {IP} - CorrelationId: {CorrelationId}", ip, correlationId);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Too Many Requests",
                    message = "Rate limit exceeded. Maximum 100 requests per minute per IP."
                });
                return;
            }

            // Check for suspicious patterns in request
            if (IsSuspiciousRequest(context))
            {
                _logger.LogWarning("Suspicious request detected from {IP}: {Method} {Path} - CorrelationId: {CorrelationId}",
                    context.Connection.RemoteIpAddress, context.Request.Method, context.Request.Path, correlationId);
                
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Bad Request",
                    message = "Request contains suspicious content"
                });
                return;
            }

            // Check for SQL injection patterns
            if (ContainsSqlInjectionPatterns(context))
            {
                _logger.LogWarning("Potential SQL injection detected from {IP}: {Method} {Path} {Query} - CorrelationId: {CorrelationId}",
                    context.Connection.RemoteIpAddress, context.Request.Method, context.Request.Path, context.Request.QueryString, correlationId);
                
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Bad Request",
                    message = "Request contains invalid characters"
                });
                return;
            }

            await _next(context);
        }

        private bool IsSuspiciousRequest(HttpContext context)
        {
            // Check User-Agent for suspicious patterns
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent) || 
                userAgent.Contains("sqlmap") || 
                userAgent.Contains("nmap") ||
                userAgent.Contains("nikto"))
            {
                return true;
            }

            // Check for unusual HTTP methods
            var method = context.Request.Method;
            if (!new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS", "HEAD" }.Contains(method))
            {
                return true;
            }

            return false;
        }

        private bool ContainsSqlInjectionPatterns(HttpContext context)
        {
            var sqlPatterns = new[]
            {
                @"(\b(union|select|insert|update|delete|drop|create|alter|exec|execute)\b)",
                @"(\b(or|and)\b\s+\d+\s*=\s*\d+)",
                @"('(\s*(or|and)\s+.*--))",
                @"(\bscript\b)",
                @"(<script)",
                @"(javascript:)",
                @"(on\w+\s*=)"
            };

            // Check query parameters
            foreach (var param in context.Request.Query)
            {
                var value = param.Value.ToString();
                if (sqlPatterns.Any(pattern => Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase)))
                {
                    return true;
                }
            }

            // Check headers
            foreach (var header in context.Request.Headers)
            {
                var value = header.Value.ToString();
                if (sqlPatterns.Any(pattern => Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}