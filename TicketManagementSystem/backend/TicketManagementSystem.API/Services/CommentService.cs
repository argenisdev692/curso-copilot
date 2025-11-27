using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for comment business logic operations
    /// </summary>
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<CommentService> logger) : base(logger)
        {
            _commentRepository = commentRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<CommentDto>> GetByTicketIdAsync(int ticketId, CancellationToken ct)
        {
            LogInformation("Getting comments for ticket {TicketId}", ticketId);

            var comments = await _commentRepository.GetByTicketIdAsync(ticketId, ct);
            return _mapper.Map<List<CommentDto>>(comments);
        }

        /// <inheritdoc />
        public async Task<CommentDto> AddAsync(int ticketId, CreateCommentDto dto, int createdByUserId, CancellationToken ct)
        {
            LogInformation("Adding comment to ticket {TicketId} by user {UserId}", ticketId, createdByUserId);

            // Validate ticket exists
            var ticket = await _ticketRepository.GetByIdAsync(ticketId, false, ct);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticketId} not found");
            }

            // Validate user exists and is active
            var user = await _userRepository.GetByIdAsync(createdByUserId);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            var comment = _mapper.Map<Comment>(dto);
            comment.TicketId = ticketId;
            comment.CreatedById = createdByUserId;

            var addedComment = await _commentRepository.AddAsync(comment, ct);
            LogInformation("Comment {CommentId} added to ticket {TicketId}", addedComment.Id, ticketId);

            return _mapper.Map<CommentDto>(addedComment);
        }

        /// <inheritdoc />
        public async Task<CommentDto> UpdateAsync(int commentId, UpdateCommentDto dto, int updatedByUserId, CancellationToken ct)
        {
            LogInformation("Updating comment {CommentId} by user {UserId}", commentId, updatedByUserId);

            var comment = await _commentRepository.GetByIdAsync(commentId, ct);
            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }

            // Validate user is the author or admin
            var user = await _userRepository.GetByIdAsync(updatedByUserId);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            if (comment.CreatedById != updatedByUserId && user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only the comment author or admin can update the comment");
            }

            _mapper.Map(dto, comment);
            await _commentRepository.UpdateAsync(comment, ct);

            LogInformation("Comment {CommentId} updated", commentId);
            return _mapper.Map<CommentDto>(comment);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int commentId, int deletedByUserId, CancellationToken ct)
        {
            LogInformation("Deleting comment {CommentId} by user {UserId}", commentId, deletedByUserId);

            var comment = await _commentRepository.GetByIdAsync(commentId, ct);
            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }

            // Validate user is the author or admin
            var user = await _userRepository.GetByIdAsync(deletedByUserId);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            if (comment.CreatedById != deletedByUserId && user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only the comment author or admin can delete the comment");
            }

            // Soft delete
            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentRepository.UpdateAsync(comment, ct);
            LogInformation("Comment {CommentId} soft deleted", commentId);
        }
    }
}