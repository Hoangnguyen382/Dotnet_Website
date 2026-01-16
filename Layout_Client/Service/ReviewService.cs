using System.Net.Http.Json;
using Layout_Client.Model.DTO;
using Layout_Client.Models.DTO;

namespace Layout_Client.Service;

public class ReviewService
{
    private readonly AuthHttpClientFactory _factory;

    public ReviewService(AuthHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<ReviewResponseDTO>> GetReviewsByRestaurantIdAsync(int restaurantId)
    {
        var client = await _factory.CreateClientAsync();
        var url = $"api/Reviews/restaurant/{restaurantId}";
        return await client.GetFromJsonAsync<List<ReviewResponseDTO>>(url);
    }

    public async Task<List<ReviewResponseDTO>> GetReviewsByMenuItemIdAsync(int menuItemId)
    {
        var client = await _factory.CreateClientAsync();
        var url = $"api/Reviews/menuitem/{menuItemId}";
        return await client.GetFromJsonAsync<List<ReviewResponseDTO>>(url);
    }
    public async Task<List<MenuItemRatingDTO>> GetTopRatedMenuItemsAsync(int top)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<List<MenuItemRatingDTO>>($"api/Reviews/top-menuitems?top={top}");
    }
    public async Task<List<RestaurantRatingDTO>> GetTopRatedRestaurantsAsync(int top)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<List<RestaurantRatingDTO>>($"api/Reviews/top-restaurants?top={top}");
    }

    public async Task<List<ReviewResponseDTO>> GetAllReviewsAsync()
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<List<ReviewResponseDTO>>("api/Reviews");
    }
    public async Task<double> GetAverageRatingForRestaurantAsync(int restaurantId)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<double>($"api/Reviews/average/restaurant/{restaurantId}");
    }

    public async Task<double> GetAverageRatingForMenuItemAsync(int menuItemId)
    {
        var client = await _factory.CreateClientAsync();
        return await client.GetFromJsonAsync<double>($"api/Reviews/average/menuitem/{menuItemId}");
    }

    public async Task<bool> CreateRestaurantReviewAsync(ReviewRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PostAsJsonAsync($"api/Reviews/restaurant", dto);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> CreateMenuItemReviewAsync(ReviewRequestDTO dto)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.PostAsJsonAsync($"api/Reviews/menuitem", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        var client = await _factory.CreateClientAsync();
        var response = await client.DeleteAsync($"api/Reviews/{reviewId}");
        return response.IsSuccessStatusCode;
    }
    
}
