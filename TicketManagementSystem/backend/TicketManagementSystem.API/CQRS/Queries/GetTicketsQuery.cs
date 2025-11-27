using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Query to get paginated tickets
    /// </summary>
    public record GetTicketsQuery(GetTicketsQueryParameters Parameters) : IRequest<Result<PagedResponse<TicketDto>>>;
}