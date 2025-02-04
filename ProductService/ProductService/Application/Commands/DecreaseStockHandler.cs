using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record DecreaseStockCommand(Guid ProductId, int Quantity) : IRequest<bool>;

    public class DecreaseStockHandler : IRequestHandler<DecreaseStockCommand, bool>
    {
        private readonly IProductRepository _repository;

        public DecreaseStockHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DecreaseStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);
            if (product == null || product.StockQuantity < request.Quantity)
                return false; // Вернем false, если товара недостаточно

            product.StockQuantity -= request.Quantity;
            await _repository.UpdateAsync(product);
            return true; // Успешное уменьшение товара
        }
    }
}