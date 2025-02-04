using MediatR;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using OrderService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Commands
{
    public record CreateOrderCommand(List<OrderItemDto> Items) : IRequest<Guid>;

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Items = request.Items.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                TotalPrice = request.Items.Sum(i => i.Price * i.Quantity)
            };

            await _orderRepository.AddAsync(order);
            return order.Id;
        }
    }
}