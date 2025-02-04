using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Commands
{
    public record DeleteProductCommand(Guid ProductId) : IRequest;

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found");

            product.DeleteDate = DateTime.UtcNow;

            await _repository.UpdateAsync(product);

            return Unit.Value; // Возвращаем Unit.Value, так как метод должен возвращать Task<Unit>
        }
    }
}