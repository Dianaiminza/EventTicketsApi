using EventsTicket.Domain.Entities;
using EventsTicket.Infrastructure.Repository.Abstractions;
using EventTicketsApi.Application.Boundary.Responses;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace EventTicketsApi.Application.Features.Queries;



public class GetEventsTicketQuery : IRequest<Result<List<EventTicketResponse>>>
{
    public GetEventsTicketQuery()
    {
    }
}

internal sealed class GetEventsTicketQueryHandler : IRequestHandler<GetEventsTicketQuery, Result<List<EventTicketResponse>>>
{
    private readonly IRepositoryUnit _repository;

    public GetEventsTicketQueryHandler(IRepositoryUnit repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<EventTicketResponse>>> Handle(GetEventsTicketQuery request, CancellationToken cancellationToken)
    {
        var eventTickets = await _repository
      .Entity<EventTicket>()
      .AsNoTracking()
      .ToListAsync(cancellationToken);

        var response = eventTickets
          .Select(ticket => new EventTicketResponse
          {
              Id = ticket.Id,
              EventName = ticket.EventName,
              Venue = ticket.Venue,
              EventDate = ticket.EventDate,
              Price = ticket.Price,
              AvailableTickets = ticket.AvailableTickets
          }).ToList();
       
        return await Result<List<EventTicketResponse>>.SuccessAsync(response, "Successfully fetched event tickets");
    }
}
