using System.Net.Http.Json;
using Layout_Client.Model.DTO;
using Layout_Client.Models.DTO;
using Layout_Client.Models.DTOs;

namespace Layout_Client.Service
{
    public class OrderService
    {
        private readonly AuthHttpClientFactory _factory;

        public OrderService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<(OrderResponseDTO? order, string? errorMessage)> CreateOrderAsync(int restaurantID, OrderRequestDTO orderDto, List<OrderDetailRequestDTO> orderDetails)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PostAsJsonAsync(
                $"api/orders?restaurantID={restaurantID}",
                new { Order = orderDto, OrderDetails = orderDetails });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OrderResponseDTO>();
                return (result, null);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (null, error);
            }
        }
        public async Task<List<OrderResponseDTO?>> GetOrderByUserIdAsync()
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/orders/user";
            return await client.GetFromJsonAsync<List<OrderResponseDTO>>(url);
        }
        public async Task<List<OrderDetailResponseDTO?>> GetOrderDetailByOrderIdAsync(int orderId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<OrderDetailResponseDTO?>>($"api/orderdetails/order/{orderId}");
        }
    }
}
