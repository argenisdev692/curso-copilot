using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Services
{
    public interface ITicketMetricsService
    {
        Task<TicketMetrics> GetMetricsAsync(int userId, DateTime from, DateTime to);
    }
}