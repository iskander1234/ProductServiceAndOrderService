using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record UpdateProductCommand(Guid ProductId, string Name, decimal Price) : IRequest;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found");

            product.Name = request.Name;
            product.Price = request.Price;

            await _repository.UpdateAsync(product);

            return Unit.Value; // Возвращаем Unit.Value, так как метод должен возвращать Task<Unit>
        }
    }
}