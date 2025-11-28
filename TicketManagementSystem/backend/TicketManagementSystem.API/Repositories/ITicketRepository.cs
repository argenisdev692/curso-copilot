using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Repository interface for ticket data operations
    /// </summary>
    public interface ITicketRepository
    {
        /// <summary>
        /// Get a ticket by ID
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <returns>Ticket or null</returns>
        Task<Ticket?> GetByIdAsync(int id);

        /// <summary>
        /// Get paginated tickets with filters
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Paged response of tickets</returns>
        Task<PagedResponse<Ticket>> GetAllAsync(GetTicketsQueryParameters parameters, CancellationToken ct);

        /// <summary>
        /// Get a ticket by ID with optional relations
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <param name="includeRelations">Whether to include related entities</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ticket or null</returns>
        Task<Ticket?> GetByIdAsync(int id, bool includeRelations, CancellationToken ct);

        /// <summary>
        /// Add a new ticket
        /// </summary>
        /// <param name="ticket">Ticket to add</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Added ticket</returns>
        Task<Ticket> AddAsync(Ticket ticket, CancellationToken ct);

        /// <summary>
        /// Update an existing ticket
        /// </summary>
        /// <param name="ticket">Ticket to update</param>
        /// <param name="ct">Cancellation token</param>
        Task UpdateAsync(Ticket ticket, CancellationToken ct);

        /// <summary>
        /// Get paginated tickets optimized with read models
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Paged response of ticket read models</returns>
        Task<PagedResponse<TicketReadModel>> GetAllOptimizedAsync(GetTicketsQueryParameters parameters, CancellationToken ct);
    }
}