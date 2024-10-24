using EventsTicket.Domain.Entities;
using EventsTicket.Infrastructure.Repository.Abstractions;
using EventTicketsApi.Application.Boundary.Requests;
using EventTicketsApi.Application.Boundary.Responses;
using Infrastructure.shared.Wrapper;
using Infrastructure.shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventTicketsApi.Application.Features.Commands;


public class UpdateEventTicketCommand : IRequest<Result<EventTicketResponse>>
{
    public UpdateEventTicketRequest UpdateRequest { get; }

    public UpdateEventTicketCommand(UpdateEventTicketRequest updateRequest)
    {
        UpdateRequest = updateRequest;
    }
}

internal sealed class UpdateEventTicketCommandHandler : IRequestHandler<UpdateEventTicketCommand, Result<EventTicketResponse>>
{
    private readonly IRepositoryUnit _repository;

    public UpdateEventTicketCommandHandler(IRepositoryUnit repository)
    {
        _repository = repository;
    }

    public async Task<Result<EventTicketResponse>> Handle(UpdateEventTicketCommand request, CancellationToken cancellationToken)
    {
        var eventTicket = await _repository
          .Entity<EventTicket>()
          .Where(c => c.Id == request.UpdateRequest.Id)
          .TrackChanges(true)
          .FirstOrDefaultAsync(cancellationToken)
          .EnsureExistsAsync($"Event ticket with id: {request.UpdateRequest.Id} not found");

        eventTicket.EventName = request.UpdateRequest.EventName;
        eventTicket.Venue = request.UpdateRequest.Venue;
        eventTicket.EventDate = request.UpdateRequest.EventDate;
        eventTicket.Price = request.UpdateRequest.Price;
        eventTicket.AvailableTickets = request.UpdateRequest.AvailableTickets;

        _repository.DbContext().EventTickets.Update(eventTicket);
        await _repository.SaveAsync("Failed to update event ticket", cancellationToken);
        var response = new EventTicketResponse { Id = eventTicket.Id, 
         EventName = request.UpdateRequest.EventName,
        Venue = request.UpdateRequest.Venue,
        EventDate = request.UpdateRequest.EventDate,
        Price = request.UpdateRequest.Price,
        AvailableTickets = request.UpdateRequest.AvailableTickets
    };
        return await Result<EventTicketResponse>.SuccessAsync(response, "Event ticket updated successfully");
    }
}
