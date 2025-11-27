using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Base class for all services providing common functionality
    /// </summary>
    public abstract class BaseService
    {
        protected readonly ILogger Logger;

        protected BaseService(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logs information with structured logging
        /// </summary>
        protected void LogInformation(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }

        /// <summary>
        /// Logs successful operations with structured data
        /// </summary>
        protected void LogOperationSuccess(int? ticketId = null, int? userId = null, string action = "", TimeSpan? elapsedTime = null)
        {
            var logData = new
            {
                TicketId = ticketId,
                UserId = userId,
                Action = action,
                ElapsedTime = elapsedTime?.TotalMilliseconds
            };
            Logger.LogInformation("Operation successful: {@LogData}", logData);
        }

        /// <summary>
        /// Logs errors with structured data and stack trace
        /// </summary>
        protected void LogOperationError(Exception ex, int? ticketId = null, int? userId = null, string action = "")
        {
            var logData = new
            {
                TicketId = ticketId,
                UserId = userId,
                Action = action
            };
            Logger.LogError(ex, "Operation failed: {@LogData}", logData);
        }

        /// <summary>
        /// Logs warnings with structured logging
        /// </summary>
        protected void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        /// <summary>
        /// Logs errors with structured logging
        /// </summary>
        protected void LogError(Exception ex, string message, params object[] args)
        {
            Logger.LogError(ex, message, args);
        }
    }
}