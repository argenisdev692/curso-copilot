using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Interface for composing email messages
/// </summary>
public interface IEmailComposer
{
    EmailMessage ComposeTicketAssignmentNotification(int ticketId, string assigneeEmail, Ticket ticket);
}

/// <summary>
/// Implementation of email composer
/// </summary>
public class EmailComposer : IEmailComposer
{
    public EmailMessage ComposeTicketAssignmentNotification(int ticketId, string assigneeEmail, Ticket ticket)
    {
        var subject = $"Ticket Assigned: {ticket.Title}";
        var body = $@"
            <h2>Ticket Assignment Notification</h2>
            <p>You have been assigned a new ticket:</p>
            <ul>
                <li><strong>Title:</strong> {ticket.Title}</li>
                <li><strong>Priority:</strong> {ticket.Priority}</li>
                <li><strong>URL:</strong> <a href='https://yourapp.com/tickets/{ticket.Id}'>View Ticket</a></li>
            </ul>
            <p>Please review and take action as needed.</p>
        ";

        return new EmailMessage
        {
            To = assigneeEmail,
            Subject = subject,
            Body = body,
            TicketId = ticketId
        };
    }
}