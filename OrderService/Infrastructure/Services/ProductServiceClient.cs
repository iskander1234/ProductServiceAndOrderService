using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using OrderService.Domain.Interfaces;

namespace OrderService.Infrastructure.Services
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DecreaseStockAsync(Guid productId, int quantity)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(quantity), Encoding.UTF8, "application/json"
            );

            var response = await _httpClient.PutAsync($"api/products/{productId}/decrease-stock", jsonContent);

            if (response.IsSuccessStatusCode)
                return true;

            Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}"); // Логируем ошибку
            return false;
        }
    }
}