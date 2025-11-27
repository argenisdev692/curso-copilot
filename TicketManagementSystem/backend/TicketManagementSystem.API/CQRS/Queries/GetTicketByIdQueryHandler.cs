using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Queries
{
    /// <summary>
    /// Handler for GetTicketByIdQuery
    /// </summary>
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, Result<TicketDto>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public GetTicketByIdQueryHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<TicketDto>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}