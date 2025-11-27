using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Query to get a ticket by ID
    /// </summary>
    public record GetTicketByIdQuery(int Id) : IRequest<Result<TicketDto>>;
}