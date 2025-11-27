namespace TicketManagementSystem.API.Services
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int TicketId { get; set; }
    }
}