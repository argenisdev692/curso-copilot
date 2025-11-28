using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Custom telemetry initializer for Application Insights
/// </summary>
public class CustomTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            // Add custom properties
            telemetry.Context.Properties["Application"] = "TicketManagementSystem";
            telemetry.Context.Properties["Environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            telemetry.Context.Properties["UserAgent"] = context.Request.Headers["User-Agent"].ToString();
            telemetry.Context.Properties["CorrelationId"] = context.Request.Headers["X-Correlation-ID"].ToString();

            // Add user information if available
            if (context.User.Identity?.IsAuthenticated == true)
            {
                telemetry.Context.User.Id = context.User.FindFirst("sub")?.Value;
                telemetry.Context.User.AccountId = context.User.FindFirst("email")?.Value;
            }
        }
    }
}