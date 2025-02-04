using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record IncreaseStockCommand(Guid ProductId, int Quantity) : IRequest<Unit>;

    public class IncreaseStockHandler : IRequestHandler<IncreaseStockCommand, Unit>
    {
        private readonly IProductRepository _repository;

        public IncreaseStockHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(IncreaseStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found");

            product.StockQuantity += request.Quantity;
            await _repository.UpdateAsync(product);

            return Unit.Value;
        }
    }
}