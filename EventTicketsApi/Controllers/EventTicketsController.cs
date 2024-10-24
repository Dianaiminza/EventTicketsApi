using EventTicketsApi.Application.Boundary.Requests;
using EventTicketsApi.Application.Boundary.Responses;
using EventTicketsApi.Application.Features.Commands;
using EventTicketsApi.Application.Features.Queries;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EventTicketsApi.Controllers
{
 /// <summary>
 /// Event Tickets
 /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/eventtickets")]
    [ApiController]
    public class EventTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Event Tickets.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<Result<List<EventTicketResponse>>>> GetEventTicketsAsync()
        {
            var response = await _mediator.Send(new GetEventsTicketQuery());
            return Ok(response);
        }

        // <summary>
        // Get a single  Event Ticket
        // </summary>
        // <returns></returns>
        [HttpGet("{eventId}")]
        public async Task<ActionResult<Result<EventTicketResponse>>> EventTicketsByIdAsync(
            [FromRoute] long eventId)
        {
            var response = await _mediator.Send(new GetEventTicketsByIdQuery(eventId));
            return Ok(response);
        }

        /// <summary>
        /// Create Event ticket.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result<EventTicketResponse>>> CreateEventTicketsAsync(
          [FromBody] CreateEventTicketRequest request)
        {
            var command = new CreateEventTicketCommand(request);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Update  Event ticket.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{eventId}")]
        public async Task<ActionResult<Result<EventTicketResponse>>> UpdateEventTicketsAsync(
            [FromRoute] long eventId,
          [FromBody] UpdateEventTicketRequest request)
        {
            request.Id = eventId;
            var command = new UpdateEventTicketCommand(request);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Delete Event ticket.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<ActionResult<Result<string>>> DeleteEventTickets([FromBody] EventTicketDeleteRequest request)
        {
            var response = await _mediator.Send(new DeleteEventTicketCommand(request));
            return Ok(response);
        }
    }
}
