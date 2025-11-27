using System.Net;
using System.Net.Mail;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    public class EmailNotificationService : BackgroundService
    {
        private readonly Channel<EmailMessage> _emailChannel = Channel.CreateUnbounded<EmailMessage>();
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(
            ILogger<EmailNotificationService> logger,
            IServiceScopeFactory scopeFactory,
            IOptions<SmtpSettings> smtpSettings)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task EnqueueEmailAsync(EmailMessage emailMessage)
        {
            await _emailChannel.Writer.WriteAsync(emailMessage);
        }

        public async Task SendTicketAssignmentNotificationAsync(int ticketId, string assigneeEmail)
        {
            using var scope = _scopeFactory.CreateScope();
            var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();

            var ticket = await ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket {TicketId} not found for email notification", ticketId);
                return;
            }

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

            var emailMessage = new EmailMessage
            {
                To = assigneeEmail,
                Subject = subject,
                Body = body,
                TicketId = ticketId
            };

            await EnqueueEmailAsync(emailMessage);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var emailMessage in _emailChannel.Reader.ReadAllAsync(stoppingToken))
            {
                await SendEmailWithRetryAsync(emailMessage, stoppingToken);
            }
        }

        private async Task SendEmailWithRetryAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(3, onRetry: (exception, retryCount) =>
                {
                    _logger.LogWarning(exception, "Failed to send email to {To} for ticket {TicketId}, retry {RetryCount}", emailMessage.To, emailMessage.TicketId, retryCount);
                });

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                    {
                        Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                        EnableSsl = _smtpSettings.EnableSsl
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                        Subject = emailMessage.Subject,
                        Body = emailMessage.Body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(emailMessage.To);

                    await smtpClient.SendMailAsync(mailMessage, cancellationToken);
                });

                _logger.LogInformation("Email sent successfully to {To} for ticket {TicketId}", emailMessage.To, emailMessage.TicketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} for ticket {TicketId} after retries", emailMessage.To, emailMessage.TicketId);
            }
        }
    }
}