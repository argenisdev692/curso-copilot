using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Helper for monitoring query performance
    /// </summary>
    public static class QueryPerformanceMonitor
    {
        private static readonly double SlowQueryThresholdMs = 1000; // 1 second

        /// <summary>
        /// Executes a query with performance monitoring
        /// </summary>
        public static async Task<T> MonitorQueryAsync<T>(
            Func<Task<T>> queryFunc,
            string queryName,
            ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await queryFunc();
                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > SlowQueryThresholdMs)
                {
                    logger.LogWarning("Slow query detected: {QueryName} took {ElapsedMs}ms",
                        queryName, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    logger.LogDebug("Query {QueryName} executed in {ElapsedMs}ms",
                        queryName, stopwatch.ElapsedMilliseconds);
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "Query {QueryName} failed after {ElapsedMs}ms",
                    queryName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        /// <summary>
        /// Executes a query with performance monitoring and additional context
        /// </summary>
        public static async Task<T> MonitorQueryAsync<T>(
            Func<Task<T>> queryFunc,
            string queryName,
            ILogger logger,
            params (string Key, object Value)[] context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await queryFunc();
                stopwatch.Stop();

                var contextString = string.Join(", ", context.Select(c => $"{c.Key}={c.Value}"));

                if (stopwatch.ElapsedMilliseconds > SlowQueryThresholdMs)
                {
                    logger.LogWarning("Slow query detected: {QueryName} ({Context}) took {ElapsedMs}ms",
                        queryName, contextString, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    logger.LogDebug("Query {QueryName} ({Context}) executed in {ElapsedMs}ms",
                        queryName, contextString, stopwatch.ElapsedMilliseconds);
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var contextString = string.Join(", ", context.Select(c => $"{c.Key}={c.Value}"));
                logger.LogError(ex, "Query {QueryName} ({Context}) failed after {ElapsedMs}ms",
                    queryName, contextString, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}