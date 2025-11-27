using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    /// <summary>
    /// Advanced query helpers for complex LINQ operations
    /// </summary>
    public static class QueryHelpers
    {
        /// <summary>
        /// Creates a dynamic filter expression for tickets based on search criteria
        /// </summary>
        public static Expression<Func<Ticket, bool>> CreateTicketFilterExpression(
            string? status = null,
            string? priority = null,
            int? assignedToId = null,
            string? search = null,
            DateTime? createdAfter = null,
            DateTime? createdBefore = null)
        {
            Expression<Func<Ticket, bool>> predicate = t => true; // Start with always true

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Status>(status, true, out var statusEnum))
            {
                var statusParam = Expression.Parameter(typeof(Ticket), "t");
                var statusProperty = Expression.Property(statusParam, nameof(Ticket.Status));
                var statusValue = Expression.Constant(statusEnum);
                var statusCondition = Expression.Equal(statusProperty, statusValue);
                predicate = CombinePredicates(predicate, Expression.Lambda<Func<Ticket, bool>>(statusCondition, statusParam));
            }

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<Priority>(priority, true, out var priorityEnum))
            {
                var priorityParam = Expression.Parameter(typeof(Ticket), "t");
                var priorityProperty = Expression.Property(priorityParam, nameof(Ticket.Priority));
                var priorityValue = Expression.Constant(priorityEnum);
                var priorityCondition = Expression.Equal(priorityProperty, priorityValue);
                predicate = CombinePredicates(predicate, Expression.Lambda<Func<Ticket, bool>>(priorityCondition, priorityParam));
            }

            if (assignedToId.HasValue)
            {
                Expression<Func<Ticket, bool>> assignedFilter = t => t.AssignedToId == assignedToId.Value;
                predicate = CombinePredicates(predicate, assignedFilter);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchTerm = search.ToLower();
                Expression<Func<Ticket, bool>> searchFilter = t =>
                    t.Title.ToLower().Contains(searchTerm) ||
                    t.Description.ToLower().Contains(searchTerm);
                predicate = CombinePredicates(predicate, searchFilter);
            }

            if (createdAfter.HasValue)
            {
                Expression<Func<Ticket, bool>> dateFilter = t => t.CreatedAt >= createdAfter.Value;
                predicate = CombinePredicates(predicate, dateFilter);
            }

            if (createdBefore.HasValue)
            {
                Expression<Func<Ticket, bool>> dateFilter = t => t.CreatedAt <= createdBefore.Value;
                predicate = CombinePredicates(predicate, dateFilter);
            }

            return predicate;
        }

        /// <summary>
        /// Combines two predicates with AND operation
        /// </summary>
        private static Expression<Func<T, bool>> CombinePredicates<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = Expression.AndAlso(
                Expression.Invoke(left, parameter),
                Expression.Invoke(right, parameter));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// Creates an optimized query for ticket analytics
        /// </summary>
        public static IQueryable<TicketAnalyticsDto> CreateTicketAnalyticsQuery(
            this ApplicationDbContext context,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = context.Tickets
                .AsNoTracking()
                .Where(t => !startDate.HasValue || t.CreatedAt >= startDate.Value)
                .Where(t => !endDate.HasValue || t.CreatedAt <= endDate.Value)
                .GroupBy(t => new { t.Status, t.Priority })
                .Select(g => new TicketAnalyticsDto
                {
                    Status = g.Key.Status.ToString(),
                    Priority = g.Key.Priority.ToString(),
                    Count = g.Count(),
                    AverageAgeInDays = g.Average(t => EF.Functions.DateDiffDay(t.CreatedAt, DateTime.UtcNow))
                });

            return query;
        }

        /// <summary>
        /// Optimized query for getting tickets with comment counts
        /// </summary>
        public static IQueryable<TicketWithCommentCount> CreateTicketsWithCommentCountQuery(
            this ApplicationDbContext context,
            GetTicketsQueryParameters parameters)
        {
            var query = from t in context.Tickets.AsNoTracking()
                       join c in context.Comments.AsNoTracking()
                           on t.Id equals c.TicketId into comments
                       select new TicketWithCommentCount
                       {
                           Ticket = t,
                           CommentCount = comments.Count()
                       };

            // Apply filters using the helper method
            var filterExpression = CreateTicketFilterExpression(
                parameters.Status,
                parameters.Priority,
                parameters.AssignedTo,
                parameters.Search);

            query = query.Where(tcc => filterExpression.Compile()(tcc.Ticket));

            return query;
        }
    }

    /// <summary>
    /// DTO for ticket analytics
    /// </summary>
    public class TicketAnalyticsDto
    {
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int Count { get; set; }
        public double AverageAgeInDays { get; set; }
    }

    /// <summary>
    /// DTO for ticket with comment count
    /// </summary>
    public class TicketWithCommentCount
    {
        public Ticket Ticket { get; set; } = null!;
        public int CommentCount { get; set; }
    }
}