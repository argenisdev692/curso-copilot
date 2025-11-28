using Prometheus;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Interface for metrics service
/// </summary>
public interface IMetricsService
{
    Task<Dictionary<string, object>> GetCustomMetricsAsync();
    Task<Dictionary<string, object>> GetDoraMetricsAsync();
    void RecordRequestDuration(string endpoint, double duration);
    void RecordDeployment();
    void RecordTestResult(bool success);
    void RecordIncidentResolved(TimeSpan resolutionTime);
}

/// <summary>
/// Custom metrics service implementation
/// </summary>
public class MetricsService : IMetricsService
{
    private readonly Counter _requestsTotal = Metrics.CreateCounter(
        "ticket_management_requests_total",
        "Total number of requests",
        new CounterConfiguration
        {
            LabelNames = new[] { "method", "endpoint", "status_code" }
        });

    private readonly Histogram _requestDuration = Metrics.CreateHistogram(
        "ticket_management_request_duration_seconds",
        "Request duration in seconds",
        new HistogramConfiguration
        {
            LabelNames = new[] { "method", "endpoint" },
            Buckets = Histogram.ExponentialBuckets(0.1, 2, 10)
        });

    private readonly Counter _deploymentsTotal = Metrics.CreateCounter(
        "ticket_management_deployments_total",
        "Total number of deployments");

    private readonly Counter _testsTotal = Metrics.CreateCounter(
        "ticket_management_tests_total",
        "Total number of tests",
        new CounterConfiguration
        {
            LabelNames = new[] { "result" }
        });

    private readonly Histogram _incidentResolutionTime = Metrics.CreateHistogram(
        "ticket_management_incident_resolution_time_seconds",
        "Time to resolve incidents in seconds");

    private readonly Gauge _activeConnections = Metrics.CreateGauge(
        "ticket_management_active_connections",
        "Number of active connections");

    public void RecordRequestDuration(string endpoint, double duration)
    {
        _requestDuration.WithLabels("GET", endpoint).Observe(duration);
    }

    public void RecordDeployment()
    {
        _deploymentsTotal.Inc();
    }

    public void RecordTestResult(bool success)
    {
        _testsTotal.WithLabels(success ? "passed" : "failed").Inc();
    }

    public void RecordIncidentResolved(TimeSpan resolutionTime)
    {
        _incidentResolutionTime.Observe(resolutionTime.TotalSeconds);
    }

    public Task<Dictionary<string, object>> GetCustomMetricsAsync()
    {
        var metrics = new Dictionary<string, object>
        {
            ["requests_total"] = _requestsTotal.Value,
            ["deployments_total"] = _deploymentsTotal.Value,
            ["tests_total"] = _testsTotal.Value,
            ["active_connections"] = _activeConnections.Value
        };

        return Task.FromResult(metrics);
    }

    public Task<Dictionary<string, object>> GetDoraMetricsAsync()
    {
        // Calculate DORA metrics
        var now = DateTime.UtcNow;
        var lastMonth = now.AddMonths(-1);

        // These would typically be calculated from historical data
        var doraMetrics = new Dictionary<string, object>
        {
            ["deployment_frequency"] = 12, // deployments per month
            ["lead_time_for_changes"] = 2.5, // hours
            ["change_failure_rate"] = 0.05, // 5%
            ["mean_time_to_recovery"] = 1.2 // hours
        };

        return Task.FromResult(doraMetrics);
    }
}