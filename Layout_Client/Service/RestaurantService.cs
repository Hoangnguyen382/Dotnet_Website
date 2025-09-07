
using System.Net.Http.Json;
using Layout_Client.Model.DTO;

namespace Layout_Client.Service;

public class RestaurantService
{
    private readonly AuthHttpClientFactory _factory;

    public RestaurantService(AuthHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<RestaurantResponseDTO>> GetAllRestaurantAsync(string? search = null,  int page = 1, int pageSize = 10)
{
    var client = await _factory.CreateClientAsync();
    var url = $"api/Restaurant?search={search}&page={page}&pageSize={pageSize}";
    return await client.GetFromJsonAsync<List<RestaurantResponseDTO>>(url);
}

    public async Task<RestaurantResponseDTO?> GetRestaurantByIdAsync(int id)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<RestaurantResponseDTO>($"api/Restaurant/{id}");
    }
}
