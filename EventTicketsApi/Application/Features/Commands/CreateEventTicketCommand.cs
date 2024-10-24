using EventsTicket.Domain.Entities;
using EventsTicket.Infrastructure.Repository.Abstractions;
using EventTicketsApi.Application.Boundary.Requests;
using EventTicketsApi.Application.Boundary.Responses;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EventTicketsApi.Application.Features.Commands;


public class CreateEventTicketCommand : IRequest<Result<EventTicketResponse>>
{
    public CreateEventTicketRequest EventCreationRequest { get; }

    public CreateEventTicketCommand(CreateEventTicketRequest eventCreationRequest)
    {
        EventCreationRequest = eventCreationRequest;
    }
}

internal sealed class CreateEventTicketCommandHandler : IRequestHandler<CreateEventTicketCommand, Result<EventTicketResponse>>
{
    private readonly IRepositoryUnit _repository;

    public CreateEventTicketCommandHandler(IRepositoryUnit repository)
    {
        _repository = repository;
    }

    public async Task<Result<EventTicketResponse>> Handle(CreateEventTicketCommand request, CancellationToken cancellationToken)
    {
        var eventTicket = new EventTicket
        {
            EventName = request.EventCreationRequest.EventName,
            Venue = request.EventCreationRequest.Venue,
            EventDate = request.EventCreationRequest.EventDate,
            Price = request.EventCreationRequest.Price,
            AvailableTickets = request.EventCreationRequest.AvailableTickets
        };

        var matchingEventTicket = await _repository
          .Entity<EventTicket>()
          .Where(c => c.EventName == request.EventCreationRequest.EventName)
          .FirstOrDefaultAsync(cancellationToken);

        if (matchingEventTicket == null)
        {
            await _repository.DbContext().EventTickets.AddAsync(eventTicket, cancellationToken);
            await _repository.SaveAsync("Failed to create event ticket", cancellationToken);
        }
        else
        {
            return await Result<EventTicketResponse>.FailAsync("A similar event ticket has already been added. Please try again", HttpStatusCode.BadRequest, null);
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
        return await Result<EventTicketResponse>.SuccessAsync(response, "Event ticket  created successfully");
    }
}
