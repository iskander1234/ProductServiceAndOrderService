using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands;
using System;
using System.Threading.Tasks;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.ProductId) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/increase-stock/{quantity}")]
        public async Task<IActionResult> IncreaseStock(Guid id, int quantity)
        {
            await _mediator.Send(new IncreaseStockCommand(id, quantity));
            return NoContent();
        }

        [HttpPut("{id}/decrease-stock/{quantity}")]
        public async Task<IActionResult> DecreaseStock(Guid id, int quantity)
        {
            await _mediator.Send(new DecreaseStockCommand(id, quantity));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            return Ok(new { Id = id, Name = "Sample Product", Price = 100, StockQuantity = 10 });
        }
    }
}