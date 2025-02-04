using MediatR;
using OrderService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Commands
{
    public record CancelOrderCommand(Guid OrderId) : IRequest<Unit>;

    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null || order.DeleteDate != null)
                throw new Exception("Order not found or already deleted");

            order.DeleteDate = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);

            return Unit.Value;
        }
    }
}