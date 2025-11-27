using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(ApplicationDbContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Priority)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PagedResponse<Ticket>> GetAllAsync(GetTicketsQueryParameters parameters, CancellationToken ct)
        {
            return await QueryPerformanceMonitor.MonitorQueryAsync(async () =>
            {
                // Start with base query using AsNoTracking for read operations
                var query = _context.Tickets
                    .AsNoTracking()
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Priority)
                    .Include(t => t.Comments) // Include comments for CommentCount calculation
                    .AsQueryable();

                // Apply dynamic filters using compilable lambda expressions
                query = ApplyFilters(query, parameters);

                // Get total count before pagination (execute count query separately)
                var totalCount = await query.CountAsync(ct);

                // Apply sorting using switch expression for better performance
                query = ApplySorting(query, parameters);

                // Apply server-side pagination
                var items = await query
                    .Skip((parameters.Page - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .ToListAsync(ct);

                return new PagedResponse<Ticket>
                {
                    Items = items,
                    TotalItems = totalCount,
                    Page = parameters.Page,
                    PageSize = parameters.PageSize
                };
            },
            "GetAllTickets",
            _logger,
            ("Page", parameters.Page),
            ("PageSize", parameters.PageSize),
            ("Status", parameters.Status ?? "All"),
            ("PriorityId", parameters.PriorityId?.ToString() ?? "All"));
        }

        private static IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query, GetTicketsQueryParameters parameters)
        {
            // Status filter - use enum parsing for type safety
            if (!string.IsNullOrEmpty(parameters.Status) &&
                Enum.TryParse<Status>(parameters.Status, true, out var statusFilter))
            {
                query = query.Where(t => t.Status == statusFilter);
            }

            // Priority filter
            if (parameters.PriorityId.HasValue && Enum.TryParse<Priority>(parameters.PriorityId.Value.ToString(), out var priorityFilter))
            {
                query = query.Where(t => t.Priority == priorityFilter);
            }

            // Assigned user filter
            if (parameters.AssignedTo.HasValue)
            {
                var assignedToId = parameters.AssignedTo.Value;
                query = query.Where(t => t.AssignedToId == assignedToId);
            }

            // Search filter - case-insensitive search in title and description
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var searchTerm = parameters.Search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchTerm) ||
                    t.Description.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private static IQueryable<Ticket> ApplySorting(IQueryable<Ticket> query, GetTicketsQueryParameters parameters)
        {
            return (parameters.SortBy?.ToLower(), parameters.SortOrder?.ToLower()) switch
            {
                ("updatedat", "asc") => query.OrderBy(t => t.UpdatedAt),
                ("updatedat", "desc") => query.OrderByDescending(t => t.UpdatedAt),
                ("priority", "asc") => query.OrderBy(t => t.Priority),
                ("priority", "desc") => query.OrderByDescending(t => t.Priority),
                ("createdat", "asc") => query.OrderBy(t => t.CreatedAt),
                ("createdat", "desc") => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt) // Default sorting
            };
        }

        public async Task<Ticket?> GetByIdAsync(int id, bool includeRelations, CancellationToken ct)
        {
            if (includeRelations)
            {
                // Use compiled query for better performance
                return await CompiledQueries.GetTicketWithRelationsAsync(_context, id);
            }

            return await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<Ticket> AddAsync(Ticket ticket, CancellationToken ct)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(ct);
            return ticket;
        }

        public async Task UpdateAsync(Ticket ticket, CancellationToken ct)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync(ct);
        }
    }
}