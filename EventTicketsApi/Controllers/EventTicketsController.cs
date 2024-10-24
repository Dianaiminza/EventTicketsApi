using EventTicketsApi.Application.Boundary.Requests;
using EventTicketsApi.Application.Boundary.Responses;
using EventTicketsApi.Application.Features.Queries;
using Infrastructure.shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EventTicketsApi.Controllers
{
 /// <summary>
 /// Events
 /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/events")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
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

        /// <summary>
        /// Create Event ticket.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("currencies/create")]
        public async Task<ActionResult<Result<EventTicketResponse>>> CreateEventTicketsAsync(
          [FromBody] CreateEventTicketRequest request)
        {
            var command = new CreateEventTicketCommand(request);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
