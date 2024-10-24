using EventTicketsApi.Application.Boundary.Responses;
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

        [HttpGet]
        public async Task<ActionResult<Result<List<EventTicketResponse>>>> GetEventsAsync()
        {
            var response = await _mediator.Send(new GetEventsTicketQuery());
            return Ok(response);
        }
    }
}
