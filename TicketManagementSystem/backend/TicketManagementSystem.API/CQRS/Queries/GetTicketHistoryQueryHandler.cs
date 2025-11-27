using MediatR;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Handler for GetTicketHistoryQuery
    /// </summary>
    public class GetTicketHistoryQueryHandler : IRequestHandler<GetTicketHistoryQuery, Result<List<TicketHistory>>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public GetTicketHistoryQueryHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<List<TicketHistory>>> Handle(GetTicketHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketHistoryAsync(request.TicketId, cancellationToken);
        }
    }
}