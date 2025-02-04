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
        private readonly IProductServiceClient _productServiceClient;

        public CreateOrderHandler(IOrderRepository orderRepository, IProductServiceClient productServiceClient)
        {
            _orderRepository = orderRepository;
            _productServiceClient = productServiceClient;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            // Проверяем и уменьшаем товары на складе
            foreach (var item in request.Items)
            {
                var success = await _productServiceClient.DecreaseStockAsync(item.ProductId, item.Quantity);
                if (!success)
                {
                    throw new Exception($"Product {item.ProductId} is out of stock or does not exist");
                }
            }

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
