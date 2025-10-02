using System.Net.Http.Json;
using Layout_Client.Model.DTO;

namespace Layout_Client.Service
{
    public class ComboService
    {
        private readonly AuthHttpClientFactory _factory;

        public ComboService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }
        public async Task<List<ComboResponseDTO>> GetComboByRestaurantAsync(int restaurantId, int page = 1, int pageSize = 10)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/Combos/public/restaurant/{restaurantId}?page={page}&pageSize={pageSize}";
            return await client.GetFromJsonAsync<List<ComboResponseDTO>>(url) ?? new();
        }
        public async Task<ComboResponseDTO?> GetComboByIdAsync(int comboId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<ComboResponseDTO>($"api/Combos/public/{comboId}");
        }
        public async Task<List<ComboDetailResponseDTO>> GetComboDetailsByIdAsync(int comboId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<ComboDetailResponseDTO>>($"api/ComboDetails/public/combo/{comboId}") ?? new();
        }
    }
}
