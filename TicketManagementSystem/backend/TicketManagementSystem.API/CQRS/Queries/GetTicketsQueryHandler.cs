using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Handler for GetTicketsQuery
    /// </summary>
    public class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, Result<PagedResponse<TicketDto>>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public GetTicketsQueryHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<PagedResponse<TicketDto>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketsAsync(request.Parameters);
        }
    }
}