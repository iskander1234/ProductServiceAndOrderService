using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record DecreaseStockCommand(Guid ProductId, int Quantity) : IRequest;

    public class DecreaseStockHandler : IRequestHandler<DecreaseStockCommand, Unit>
    {
        private readonly IProductRepository _repository;

        public DecreaseStockHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DecreaseStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found");

            if (product.StockQuantity < request.Quantity)
                throw new Exception("Not enough stock available");

            product.StockQuantity -= request.Quantity;

            await _repository.UpdateAsync(product);

            return Unit.Value; // Возвращаем Unit.Value вместо void
        }
    }
}