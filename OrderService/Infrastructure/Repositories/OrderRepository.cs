using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(Guid id) =>
            await _context.Orders
                .Include(o => o.Items) // Загружаем товары в заказе
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<List<Order>> GetAllAsync() =>
            await _context.Orders.Include(o => o.Items).ToListAsync();

        public async Task AddAsync(Order order) //  Реализуем метод
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); //  Сохранение изменений
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}