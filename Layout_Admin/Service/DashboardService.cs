using System.Net;
using System.Net.Http.Json;
using Layout_Admin.Model.DTO;
using Layout_Admin.Models.DTO;
using Layout_Admin.Service;
namespace Layout_Admin.Service
{
    public class DashboardService
    {
        private readonly AuthHttpClientFactory _factory;
        public DashboardService(AuthHttpClientFactory factory) => _factory = factory;

        public async Task<RevenueTodayDTO?> GetTodayRevenueAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<RevenueTodayDTO>($"api/Dashboard/revenue/today/{restaurantId}");
        }

        public async Task<List<RevenueWeekDTO?>> GetWeeklyRevenueAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<RevenueWeekDTO?>>($"api/Dashboard/revenue/week/{restaurantId}");
        }

        public async Task<OrderCountDTO?> GetOrderCountTodayAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<OrderCountDTO>($"api/Dashboard/orders/today/{restaurantId}");
        }

        public async Task<BestSellingItemDTO?> GetBestSellingItemTodayAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.GetAsync($"api/dashboard/best-selling-item/{restaurantId}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Không có dữ liệu
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BestSellingItemDTO>();
            }

            // Ghi log lỗi nếu cần
            Console.WriteLine("Lỗi API: " + response.StatusCode);
            return null;
        }

        public async Task<List<TopSellingItemMonthlyDTO>?> GetTopSellingItemMonthlyAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<TopSellingItemMonthlyDTO>>($"api/Dashboard/bestseller/month/{restaurantId}");
        }
    }
}