using System.Net.Http.Json;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service
{
    public class OrderService
    {
        private readonly AuthHttpClientFactory _factory;

        public OrderService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }
        public async Task<List<OrderResponseDTO?>> GetOrderByRestaurantIdAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/orders/restaurant/{restaurantId}";
            return await client.GetFromJsonAsync<List<OrderResponseDTO>>(url);
        }
        public async Task<bool> UpdateOrderAsync(int id, OrderRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PutAsJsonAsync($"api/orders/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<OrderDetailResponseDTO?>> GetOrderDetailByOrderIdAsync(int orderId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<OrderDetailResponseDTO?>>($"api/orderdetails/order/{orderId}");
        }
    }
}
