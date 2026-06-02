using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmartLogistics.Application.Shipments.Commands.CreateShipment;

namespace SmartLogistics.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShipmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentCommand command,CancellationToken cancellation)
        {
            var shipmentId = await _mediator.Send(command);
            return Ok(shipmentId);
        }

    }
}
