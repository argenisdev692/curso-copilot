using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for ticket authorization logic
    /// </summary>
    public class TicketAuthorizationService : BaseService, ITicketAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public TicketAuthorizationService(
            ApplicationDbContext context,
            IUserService userService,
            ILogger<TicketAuthorizationService> logger) : base(logger)
        {
            _context = context;
            _userService = userService;
        }

        /// <inheritdoc />
        public async Task<Result<bool>> CanAccessTicketAsync(int ticketId, int userId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDeleted);

                if (ticket == null)
                {
                    return Result<bool>.Failure("Ticket not found", "NotFound");
                }

                // User can access if they created it, are assigned to it, or are admin/agent
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return Result<bool>.Failure("User not found", "NotFound");
                }

                var hasAccess = ticket.CreatedById == userId ||
                               ticket.AssignedToId == userId ||
                               user.Role == Constants.UserRoles.Admin ||
                               user.Role == Constants.UserRoles.Agent;

                return Result<bool>.Success(hasAccess);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error checking access for ticket {TicketId} by user {UserId}", ticketId, userId);
                return Result<bool>.Failure("Error checking access permissions", "InternalError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<bool>> CanUpdateTicketAsync(int ticketId, int userId, string userRole)
        {
            try
            {
                var accessResult = await CanAccessTicketAsync(ticketId, userId);
                if (!accessResult.IsSuccess)
                {
                    return accessResult;
                }

                if (!accessResult.Value)
                {
                    return Result<bool>.Success(false);
                }

                // Additional check: Only creator, assigned user, admin, or agent can update
                var ticket = await _context.Tickets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDeleted);

                if (ticket == null)
                {
                    return Result<bool>.Failure("Ticket not found", "NotFound");
                }

                var canUpdate = ticket.CreatedById == userId ||
                               ticket.AssignedToId == userId ||
                               userRole == Constants.UserRoles.Admin ||
                               userRole == Constants.UserRoles.Agent;

                return Result<bool>.Success(canUpdate);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error checking update permission for ticket {TicketId} by user {UserId}", ticketId, userId);
                return Result<bool>.Failure("Error checking update permissions", "InternalError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<bool>> CanDeleteTicketAsync(int ticketId, int userId, string userRole)
        {
            try
            {
                // Only admins can delete tickets
                if (userRole != Constants.UserRoles.Admin)
                {
                    return Result<bool>.Success(false);
                }

                var ticket = await _context.Tickets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDeleted);

                if (ticket == null)
                {
                    return Result<bool>.Failure("Ticket not found", "NotFound");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error checking delete permission for ticket {TicketId} by user {UserId}", ticketId, userId);
                return Result<bool>.Failure("Error checking delete permissions", "InternalError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<bool>> CanViewTicketHistoryAsync(int ticketId, int userId, string userRole)
        {
            // Same rules as accessing the ticket
            return await CanAccessTicketAsync(ticketId, userId);
        }
    }
}