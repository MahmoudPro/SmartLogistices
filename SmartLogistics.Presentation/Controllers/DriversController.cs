using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLogistics.Application.Drivers.Commands.CreateDriver;

namespace SmartLogistics.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DriversController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverCommand command, CancellationToken cancellation)
        {
            var driverId = await _mediator.Send(command, cancellation);
            return Ok(driverId);
        }


    }
}
