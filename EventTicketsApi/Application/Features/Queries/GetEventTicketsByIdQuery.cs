using EventsTicket.Domain.Entities;
using EventsTicket.Infrastructure.Repository.Abstractions;
using EventTicketsApi.Application.Boundary.Responses;
using Infrastructure.shared.CustomExceptions;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventTicketsApi.Application.Features.Queries;


public class GetEventTicketsByIdQuery : IRequest<Result<EventTicketResponse>>
{
    public long TicketEventId { get; set; }

    public GetEventTicketsByIdQuery(long ticketEventId)
    {
        TicketEventId = ticketEventId;
    }

    internal sealed class
        GetEventTicketsByIdQueryHandler : IRequestHandler<GetEventTicketsByIdQuery,
            Result<EventTicketResponse>>
    {
        private readonly IRepositoryUnit _repository;

        public GetEventTicketsByIdQueryHandler(IRepositoryUnit repository)
        {
            _repository = repository;
        }

        public async Task<Result<EventTicketResponse>> Handle(
            GetEventTicketsByIdQuery request,
            CancellationToken cancellationToken)
        {
            var eventTicket = await _repository.Entity<EventTicket>()
                .Where(po => po.Id == request.TicketEventId)
                .SingleOrDefaultAsync(cancellationToken);

            if (eventTicket == null)
            {
                throw new ApiException($"Event ticket with id : {request.TicketEventId} not found");
            }
            var response = new EventTicketResponse
            {
                Id = eventTicket.Id,
                EventName = eventTicket.EventName,
                Venue = eventTicket.Venue,
                EventDate = eventTicket.EventDate,
                Price = eventTicket.Price,
                AvailableTickets = eventTicket.AvailableTickets
            };
            return await Result<EventTicketResponse>.SuccessAsync(response, "Successfully fetched event tickets");
        }
    }
}
