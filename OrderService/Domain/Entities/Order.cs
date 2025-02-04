namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime? DeleteDate { get; set; } // Soft Delete
}