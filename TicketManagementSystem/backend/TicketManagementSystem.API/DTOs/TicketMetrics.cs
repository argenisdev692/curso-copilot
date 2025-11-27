namespace TicketManagementSystem.API.DTOs
{
    public class TicketMetrics
    {
        public int TotalAssigned { get; set; }
        public int TotalResolved { get; set; }
        public double AvgResolutionTimeHours { get; set; }
    }
}