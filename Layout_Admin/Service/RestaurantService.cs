
using System.Net.Http.Json;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service;

public class RestaurantService
{
    private readonly AuthHttpClientFactory _factory;

    public RestaurantService(AuthHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<RestaurantResponseDTO>> GetAllRestaurantsByUserAsync()
    {
        var client = await _factory.CreateClientAsync();
        var url = $"api/Restaurant/user";
        return await client.GetFromJsonAsync<List<RestaurantResponseDTO>>(url);
    }

    public async Task<RestaurantResponseDTO?> GetRestaurantByIdAsync(int id)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<RestaurantResponseDTO>($"api/Restaurant/{id}");
    }

    public async Task<bool> CreateRestaurantAsync(RestaurantRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PostAsJsonAsync("api/Restaurant", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateRestaurantAsync(int id, RestaurantRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PutAsJsonAsync($"api/Restaurant/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteRestaurantAsync(int id)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.DeleteAsync($"api/Restaurant/{id}");
        return response.IsSuccessStatusCode;
    }
}
