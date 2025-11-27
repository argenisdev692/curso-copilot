using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    public class TicketMetricsService : BaseService, ITicketMetricsService
    {
        private readonly ApplicationDbContext _context;

        public TicketMetricsService(ApplicationDbContext context, ILogger<TicketMetricsService> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<TicketMetrics> GetMetricsAsync(int userId, DateTime from, DateTime to)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Total assigned tickets
                int totalAssigned = await _context.Tickets
                    .CountAsync(t => t.AssignedToId == userId && t.CreatedAt >= from && t.CreatedAt <= to);

                // Query for resolved tickets with resolution times
                var resolvedQuery = _context.Tickets
                    .Where(t => t.AssignedToId == userId && t.CreatedAt >= from && t.CreatedAt <= to)
                    .Join(_context.TicketHistories.Where(h => h.NewStatus == Status.Resolved),
                        t => t.Id,
                        h => h.TicketId,
                        (t, h) => new { Ticket = t, History = h })
                    .GroupBy(x => x.Ticket.Id)
                    .Select(g => new
                    {
                        TicketId = g.Key,
                        CreatedAt = g.First().Ticket.CreatedAt,
                        ResolutionTimeHours = (g.Min(h => h.History.ChangedAt) - g.First().Ticket.CreatedAt).TotalHours
                    });

                int totalResolved = await resolvedQuery.CountAsync();
                double avgResolutionTimeHours = totalResolved > 0 ? await resolvedQuery.AverageAsync(x => x.ResolutionTimeHours) : 0;

                var metrics = new TicketMetrics
                {
                    TotalAssigned = totalAssigned,
                    TotalResolved = totalResolved,
                    AvgResolutionTimeHours = avgResolutionTimeHours
                };

                stopwatch.Stop();
                LogInformation("Calculated ticket metrics for user {UserId} in {ElapsedMs} ms", userId, stopwatch.ElapsedMilliseconds);

                return metrics;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogError(ex, "Error calculating ticket metrics for user {UserId}", userId);
                throw;
            }
        }
    }
}