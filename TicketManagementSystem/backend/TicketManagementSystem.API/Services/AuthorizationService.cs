using System.Threading.Tasks;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for handling authorization logic
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;

        public AuthorizationService(ITicketService ticketService, IUserService userService)
        {
            _ticketService = ticketService;
            _userService = userService;
        }

        public async Task<Result> CanAccessTicketAsync(int ticketId, int userId)
        {
            var ticket = await _ticketService.GetByIdAsync(ticketId, default);
            if (ticket == null || !ticket.IsSuccess || ticket.Value == null)
                return Result.Failure("Ticket not found");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return Result.Failure("User not found");

            if (ticket.Value.CreatedById == userId || ticket.Value.AssignedToId == userId ||
                user.Role == Constants.UserRoles.Admin || user.Role == Constants.UserRoles.Agent)
            {
                return Result.Success();
            }

            return Result.Failure("Access denied");
        }

        public async Task<Result> CanUpdateTicketAsync(int ticketId, int userId)
        {
            // Similar logic, perhaps allow creators and assignees to update
            var ticket = await _ticketService.GetByIdAsync(ticketId, default);
            if (ticket == null || !ticket.IsSuccess || ticket.Value == null)
                return Result.Failure("Ticket not found");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return Result.Failure("User not found");

            if (ticket.Value.CreatedById == userId || ticket.Value.AssignedToId == userId ||
                user.Role == Constants.UserRoles.Admin || user.Role == Constants.UserRoles.Agent)
            {
                return Result.Success();
            }

            return Result.Failure("Update denied");
        }

        public async Task<Result> CanDeleteTicketAsync(int ticketId, int userId)
        {
            // Only admins can delete
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return Result.Failure("User not found");

            if (user.Role == Constants.UserRoles.Admin)
            {
                return Result.Success();
            }

            return Result.Failure("Delete denied");
        }
    }
}