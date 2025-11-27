using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Repository for comment operations
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Comment>> GetByTicketIdAsync(int ticketId, CancellationToken ct)
        {
            // Use compiled query for better performance
            var comments = CompiledQueries.GetCommentsByTicketId(_context, ticketId);
            return await Task.FromResult(comments.ToList());
        }

        /// <inheritdoc />
        public async Task<Comment?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Comments
                .AsNoTracking() // Use AsNoTracking for read operations
                .Include(c => c.CreatedBy)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        /// <inheritdoc />
        public async Task<Comment> AddAsync(Comment comment, CancellationToken ct)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(ct);
            return comment;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Comment comment, CancellationToken ct)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Comment comment, CancellationToken ct)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(ct);
        }
    }
}