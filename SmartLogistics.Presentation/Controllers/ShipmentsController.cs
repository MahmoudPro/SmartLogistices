using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmartLogistics.Application.Shipments.Commands.CreateShipment;
using SmartLogistics.Application.Shipments.Commands.AssignDriver;
using SmartLogistics.Application.Shipments.Queries.GetShipmentByTrackingNumber;

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
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentCommand command, CancellationToken cancellation)
        {
            var shipmentId = await _mediator.Send(command, cancellation);
            return Ok(shipmentId);
        }

        [HttpPut("{id}/assign-driver")]
        public async Task<IActionResult> AssignDriver(Guid id, [FromBody] Guid driverId, CancellationToken cancellation)
        {
            var command = new AssignDriverCommand(id, driverId);
            await _mediator.Send(command, cancellation);
            return NoContent();
        }

        [HttpGet("track/{trackingNumber}")]
        public async Task<IActionResult> GetShipmentByTrackingNumber(string trackingNumber, CancellationToken cancellation)
        {
            var query = new GetShipmentByTrackingNumberQuery(trackingNumber);
            var shipmentDetails = await _mediator.Send(query, cancellation);
            if (shipmentDetails == null)
                return NotFound();
            return Ok(shipmentDetails);
        }



    }
}
