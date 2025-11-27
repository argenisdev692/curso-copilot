using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Handler for GetUserTicketsQuery
    /// </summary>
    public class GetUserTicketsQueryHandler : IRequestHandler<GetUserTicketsQuery, Result<List<TicketDto>>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public GetUserTicketsQueryHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<List<TicketDto>>> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetUserTicketsAsync(request.UserId, cancellationToken);
        }
    }
}