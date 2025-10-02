using System.Net.Http.Json;
using Layout_Admin.Model.DTO;
using Layout_Admin.Models.DTO;

namespace Layout_Admin.Service
{
    public class CategoryService
    {
        private readonly AuthHttpClientFactory _factory;

        public CategoryService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }

        // Lấy danh sách category theo nhà hàng
        public async Task<List<CategoryResponseDTO>> GetCategoriesByRestaurantAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/Category?restaurantID={restaurantId}";
            return await client.GetFromJsonAsync<List<CategoryResponseDTO>>(url) ?? new();
        }
        public async Task<CategoryResponseDTO?> GetCategoryByIdAsync(int categoryId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<CategoryResponseDTO>($"api/Category/{categoryId}");
        }

        public async Task<bool> CreateCategoryAsync(CategoryRequestDTO dto, int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PostAsJsonAsync($"api/Category/create?restaurantID={restaurantId}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int categoryId, CategoryRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PutAsJsonAsync($"api/Category/{categoryId}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.DeleteAsync($"api/Category/{categoryId}");
            return response.IsSuccessStatusCode;
        }
    }
}
