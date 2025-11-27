using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for ticket authorization operations
    /// </summary>
    public interface ITicketAuthorizationService
    {
        /// <summary>
        /// Checks if a user can access a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>Result indicating access permission</returns>
        Task<Result<bool>> CanAccessTicketAsync(int ticketId, int userId);

        /// <summary>
        /// Checks if a user can update a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="userRole">User role</param>
        /// <returns>Result indicating update permission</returns>
        Task<Result<bool>> CanUpdateTicketAsync(int ticketId, int userId, string userRole);

        /// <summary>
        /// Checks if a user can delete a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="userRole">User role</param>
        /// <returns>Result indicating delete permission</returns>
        Task<Result<bool>> CanDeleteTicketAsync(int ticketId, int userId, string userRole);

        /// <summary>
        /// Checks if a user can view ticket history
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="userRole">User role</param>
        /// <returns>Result indicating history access permission</returns>
        Task<Result<bool>> CanViewTicketHistoryAsync(int ticketId, int userId, string userRole);
    }
}