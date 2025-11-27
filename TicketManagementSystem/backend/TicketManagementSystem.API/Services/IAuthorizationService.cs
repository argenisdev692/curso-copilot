using System.Threading.Tasks;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for authorization business logic
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Check if user can access a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>Result indicating access permission</returns>
        Task<Result> CanAccessTicketAsync(int ticketId, int userId);

        /// <summary>
        /// Check if user can update a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>Result indicating update permission</returns>
        Task<Result> CanUpdateTicketAsync(int ticketId, int userId);

        /// <summary>
        /// Check if user can delete a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>Result indicating delete permission</returns>
        Task<Result> CanDeleteTicketAsync(int ticketId, int userId);
    }
}