using System.Net.Http.Json;
using Layout_Admin.Models.DTO;
using Layout_Admin.Service;
namespace Layout_Admin.Service
{
    public class MenuItemService
    {
        private readonly AuthHttpClientFactory _factory;
        public MenuItemService(AuthHttpClientFactory factory) => _factory = factory;
        public async Task<List<MenuItemResponseDTO>> GetAllMenuItemAsync(int restaurantId, string? search = null, int? categoryId = null, int page = 1, int pageSize = 10)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/MenuItem?restaurantId={restaurantId}&categoryId={(categoryId ?? 0)}&page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";

            return await client.GetFromJsonAsync<List<MenuItemResponseDTO>>(url) ?? new();
        }
        public async Task<List<MenuItemResponseDTO>> GetByRestaurantAsync(int restaurantId, string? search = null, int? categoryId = null, int page = 1, int pageSize = 10)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/MenuItem/restaurant/{restaurantId}?categoryId={(categoryId ?? 0)}&page={page}&pageSize={pageSize}";
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

        public async Task<bool> CreateAsync(int restaurantId, MenuItemRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PostAsJsonAsync($"api/MenuItem/create?restaurantID={restaurantId}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, MenuItemRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PutAsJsonAsync($"api/MenuItem/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.DeleteAsync($"api/MenuItem/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}