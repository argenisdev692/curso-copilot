namespace TicketManagementSystem.API.DTOs
{
    public class DaysSinceCreationResult
    {
        public int DaysElapsed { get; set; }
        public bool IsOverdue { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}