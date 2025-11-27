using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Compiled queries for better performance on frequently executed queries
    /// </summary>
    public static class CompiledQueries
    {
        /// <summary>
        /// Compiled query for getting a ticket with all relations for read operations
        /// </summary>
        public static readonly Func<ApplicationDbContext, int, Task<Ticket?>> GetTicketWithRelationsAsync =
            EF.CompileAsyncQuery((ApplicationDbContext context, int ticketId) =>
                context.Tickets
                    .AsNoTracking()
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments!)
                        .ThenInclude(c => c.CreatedBy)
                    .Include(t => t.History!)
                        .ThenInclude(h => h.ChangedBy)
                    .FirstOrDefault(t => t.Id == ticketId));

        /// <summary>
        /// Compiled query for getting user by email (case-insensitive), including soft-deleted users
        /// </summary>
        public static readonly Func<ApplicationDbContext, string, Task<User?>> GetUserByEmailAsync =
            EF.CompileAsyncQuery((ApplicationDbContext context, string email) =>
                context.Users
                    .AsNoTracking()
                    .IgnoreQueryFilters() // Include soft-deleted users for unique email validation
                    .FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));

        /// <summary>
        /// Compiled query for checking if user exists and is active
        /// </summary>
        public static readonly Func<ApplicationDbContext, int, Task<bool>> UserExistsAndActiveAsync =
            EF.CompileAsyncQuery((ApplicationDbContext context, int userId) =>
                context.Users.Any(u => u.Id == userId && u.IsActive));

        /// <summary>
        /// Compiled query for getting comments by ticket ID with user info
        /// </summary>
        public static readonly Func<ApplicationDbContext, int, IEnumerable<Comment>> GetCommentsByTicketId =
            EF.CompileQuery((ApplicationDbContext context, int ticketId) =>
                context.Comments
                    .AsNoTracking()
                    .Include(c => c.CreatedBy)
                    .Where(c => c.TicketId == ticketId));
    }
}