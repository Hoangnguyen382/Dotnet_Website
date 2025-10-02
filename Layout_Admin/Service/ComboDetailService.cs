using System.Net.Http.Json;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service;

public class ComboDetailService
{
    private readonly AuthHttpClientFactory _factory;

    public ComboDetailService(AuthHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<ComboDetailResponseDTO>> GetDetailsByComboIdAsync(int comboId)
    {
        var client = await _factory.CreateClientAsync();
        var url = $"api/ComboDetails/combo/{comboId}";
        return await client.GetFromJsonAsync<List<ComboDetailResponseDTO>>(url) ?? new();
    }

    public async Task<ComboDetailResponseDTO?> GetDetailByIdAsync(int comboDetailId)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<ComboDetailResponseDTO>($"api/ComboDetails/{comboDetailId}");
    }

    public async Task<bool> AddDetailAsync(ComboDetailRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PostAsJsonAsync("api/ComboDetails", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDetailAsync(int comboDetailId, ComboDetailRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PutAsJsonAsync($"api/ComboDetails/{comboDetailId}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDetailAsync(int comboDetailId)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.DeleteAsync($"api/ComboDetails/{comboDetailId}");
        return response.IsSuccessStatusCode;
    }
}
