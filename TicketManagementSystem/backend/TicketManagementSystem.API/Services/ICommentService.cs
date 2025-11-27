using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for comment business logic operations
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Get all comments for a specific ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of comments</returns>
        Task<List<CommentDto>> GetByTicketIdAsync(int ticketId, CancellationToken ct);

        /// <summary>
        /// Add a new comment to a ticket
        /// </summary>
        /// <param name="ticketId">Ticket ID</param>
        /// <param name="dto">Comment creation data</param>
        /// <param name="createdByUserId">ID of the user creating the comment</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The created comment</returns>
        Task<CommentDto> AddAsync(int ticketId, CreateCommentDto dto, int createdByUserId, CancellationToken ct);

        /// <summary>
        /// Update an existing comment
        /// </summary>
        /// <param name="commentId">Comment ID</param>
        /// <param name="dto">Update data</param>
        /// <param name="updatedByUserId">ID of the user updating the comment</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The updated comment</returns>
        Task<CommentDto> UpdateAsync(int commentId, UpdateCommentDto dto, int updatedByUserId, CancellationToken ct);

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="commentId">Comment ID</param>
        /// <param name="deletedByUserId">ID of the user deleting the comment</param>
        /// <param name="ct">Cancellation token</param>
        Task DeleteAsync(int commentId, int deletedByUserId, CancellationToken ct);
    }
}