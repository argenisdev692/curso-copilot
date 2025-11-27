namespace TicketManagementSystem.API.Services
{
    public interface IPdfReportService
    {
        Task<string> GenerateClosedTicketsReportAsync(DateTime from, DateTime to);
    }
}