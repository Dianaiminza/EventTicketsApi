using EventsTicket.Domain.Entities;
using EventsTicket.Infrastructure.Repository.Abstractions;
using EventTicketsApi.Application.Boundary.Requests;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.shared.Extensions;

namespace EventTicketsApi.Application.Features.Commands;


public class DeleteEventTicketCommand : IRequest<Result<string>>
{
    public DeleteEventTicketCommand(EventTicketDeleteRequest deleteRequest)
    {
        DeleteRequest = deleteRequest;
    }

    public EventTicketDeleteRequest DeleteRequest { get; }

    internal sealed class DeleteEventTicketCommandHandler : IRequestHandler<DeleteEventTicketCommand, Result<string>>
    {
        private readonly IRepositoryUnit _repositoryUnit;

        public DeleteEventTicketCommandHandler(IRepositoryUnit repositoryUnit)
        {
            _repositoryUnit = repositoryUnit;
        }

        public async Task<Result<string>> Handle(DeleteEventTicketCommand request, CancellationToken cancellationToken)
        {
            var eventTicket = await _repositoryUnit.Entity<EventTicket>()
              .Where(tip => tip.Id == request.DeleteRequest.Id)
              .SingleOrDefaultAsync(cancellationToken)
              .EnsureExistsAsync($"Event ticket  with ID {request.DeleteRequest.Id} not Found");
       
            _repositoryUnit.DbContext().Remove(eventTicket);

            await _repositoryUnit.SaveAsync("Failed to delete event ticket ", cancellationToken);

            return await Result<string>.SuccessAsync("Event ticket  successfully deleted");
        }
    }
}
