using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllAsync();
        Task AddAsync(Order order); // ✅ Должен быть объявлен
        Task UpdateAsync(Order order);
    }
}