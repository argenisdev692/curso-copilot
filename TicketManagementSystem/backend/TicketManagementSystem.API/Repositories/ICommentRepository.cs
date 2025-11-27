using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Interface for comment repository operations
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Get all comments for a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of comments</returns>
        Task<List<Comment>> GetByTicketIdAsync(int ticketId, CancellationToken ct);

        /// <summary>
        /// Get a comment by ID
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Comment or null</returns>
        Task<Comment?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Add a new comment
        /// </summary>
        /// <param name="comment">Comment to add</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Added comment</returns>
        Task<Comment> AddAsync(Comment comment, CancellationToken ct);

        /// <summary>
        /// Update an existing comment
        /// </summary>
        /// <param name="comment">Comment to update</param>
        /// <param name="ct">Cancellation token</param>
        Task UpdateAsync(Comment comment, CancellationToken ct);

        /// <summary>
        /// Delete a comment (soft delete)
        /// </summary>
        /// <param name="comment">Comment to delete</param>
        /// <param name="ct">Cancellation token</param>
        Task DeleteAsync(Comment comment, CancellationToken ct);
    }
}