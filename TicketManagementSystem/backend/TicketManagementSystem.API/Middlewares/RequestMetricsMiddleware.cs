using System.Diagnostics;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.Middlewares;

/// <summary>
/// Middleware for capturing request metrics
/// </summary>
public class RequestMetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMetricsService _metricsService;

    public RequestMetricsMiddleware(RequestDelegate next, IMetricsService metricsService)
    {
        _next = next;
        _metricsService = metricsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var duration = stopwatch.Elapsed.TotalSeconds;

            // Record request duration
            _metricsService.RecordRequestDuration(
                context.Request.Path.Value ?? "/",
                duration);
        }
    }
}

/// <summary>
/// Extension methods for RequestMetricsMiddleware
/// </summary>
public static class RequestMetricsMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestMetrics(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestMetricsMiddleware>();
    }
}