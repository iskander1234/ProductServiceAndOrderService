using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using System;
using System.Threading.Tasks;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, orderId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await _mediator.Send(new CancelOrderCommand(id));
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(Guid id)
        {
            return Ok(new { Id = id, TotalPrice = 1000, Items = new List<object>() });
        }
    }
}