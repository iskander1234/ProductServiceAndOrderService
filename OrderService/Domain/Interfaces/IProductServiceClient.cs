namespace OrderService.Domain.Interfaces;

public interface IProductServiceClient
{
    Task<bool> DecreaseStockAsync(Guid productId, int quantity);
}