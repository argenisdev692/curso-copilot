using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for ticket business logic operations
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Get paginated tickets with optional filters and sorting
        /// </summary>
        /// <param name="parameters">Query parameters for pagination, filters, and sorting</param>
        /// <returns>Paged response containing tickets</returns>
        Task<Result<PagedResponse<TicketDto>>> GetTicketsAsync(GetTicketsQueryParameters parameters);

        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <param name="dto">Ticket creation data</param>
        /// <param name="createdByUserId">ID of the user creating the ticket</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result of the creation operation</returns>
        Task<Result<Ticket>> CreateAsync(CreateTicketDto dto, int createdByUserId, CancellationToken ct);

        /// <summary>
        /// Update an existing ticket
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="dto">Update data</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result of the update operation</returns>
        Task<Result<Ticket>> UpdateAsync(int id, UpdateTicketDto dto, CancellationToken ct);

        /// <summary>
        /// Get a ticket by ID
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result containing the ticket DTO</returns>
        Task<Result<TicketDto>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Delete a ticket (soft delete)
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        Task<Result> DeleteAsync(int id, CancellationToken ct);

        /// <summary>
        /// Get tickets for a specific user (created by or assigned to)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result containing the list of user's tickets</returns>
        Task<Result<List<TicketDto>>> GetUserTicketsAsync(int userId, CancellationToken ct);

        /// <summary>
        /// Get ticket history with optional filters and pagination
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="filter">Optional filter parameters</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result containing the paginated list of ticket history entries</returns>
        Task<Result<PagedResponse<TicketHistoryDto>>> GetTicketHistoryAsync(int ticketId, TicketHistoryFilterDto? filter, CancellationToken ct);

        /// <summary>
        /// Get ticket history (legacy method for backwards compatibility)
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result containing the list of ticket history entries</returns>
        [Obsolete("Use GetTicketHistoryAsync with filter parameter instead")]
        Task<Result<List<TicketHistory>>> GetTicketHistoryAsync(int ticketId, CancellationToken ct);
    }
}