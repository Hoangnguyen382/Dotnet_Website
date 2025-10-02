using System.Net.Http.Json;
using Layout_Client.Models.DTO;
using Layout_Client.Service;
namespace Layout_Client.Service
{
    public class MenuItemService
    {
        private readonly AuthHttpClientFactory _factory;
        public MenuItemService(AuthHttpClientFactory factory) => _factory = factory;

        public async Task<List<MenuItemResponseDTO>> GetByRestaurantAsync(int restaurantId, string? search = null, int? categoryId = null, int page = 1, int pageSize = 10)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/MenuItem/restaurant/{restaurantId}?categoryId={(categoryId ?? 0)}&page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";

            return await client.GetFromJsonAsync<List<MenuItemResponseDTO>>(url) ?? new();
        }
        public async Task<List<MenuItemResponseDTO>> GetAllMenuItemAsync(int restaurantId, string? search = null, int? categoryId = null, int page = 1, int pageSize = 10)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/MenuItem?restaurantId={restaurantId}&categoryId={(categoryId ?? 0)}&page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";

            return await client.GetFromJsonAsync<List<MenuItemResponseDTO>>(url) ?? new();
        }
        public async Task<List<CategoryResponseDTO>> GetCategoriesAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<CategoryResponseDTO>>($"api/Category?restaurantId={restaurantId}") ?? new();
        }

        public async Task<MenuItemResponseDTO?> GetByIdAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<MenuItemResponseDTO>($"api/MenuItem/{id}");
        }
    }
}