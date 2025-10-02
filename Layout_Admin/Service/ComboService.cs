using System.Net.Http.Json;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service;

public class ComboService
{
    private readonly AuthHttpClientFactory _factory;

    public ComboService(AuthHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<ComboResponseDTO>> GetCombosByRestaurantAsync(int restaurantId, int page = 1, int pageSize = 10)
    {
        var client = await _factory.CreateClientAsync();
        var url = $"api/Combos/restaurant/{restaurantId}?page={page}&pageSize={pageSize}";
        return await client.GetFromJsonAsync<List<ComboResponseDTO>>(url) ?? new();
    }

    public async Task<ComboResponseDTO?> GetComboByIdAsync(int comboId)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<ComboResponseDTO>($"api/Combos/{comboId}");
    }

    public async Task<bool> CreateComboAsync(ComboRequestDTO dto, int restaurantId)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PostAsJsonAsync($"api/Combos/restaurant/{restaurantId}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateComboAsync(int comboId, ComboRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PutAsJsonAsync($"api/Combos/{comboId}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteComboAsync(int comboId)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.DeleteAsync($"api/Combos/{comboId}");
        return response.IsSuccessStatusCode;
    }
}
