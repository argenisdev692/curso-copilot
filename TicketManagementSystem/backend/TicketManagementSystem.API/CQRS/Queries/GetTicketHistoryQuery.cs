using MediatR;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Query to get ticket history
    /// </summary>
    public record GetTicketHistoryQuery(int TicketId) : IRequest<Result<List<TicketHistory>>>;
}