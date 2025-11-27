using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Query to get tickets for a specific user
    /// </summary>
    public record GetUserTicketsQuery(int UserId) : IRequest<Result<List<TicketDto>>>;
}