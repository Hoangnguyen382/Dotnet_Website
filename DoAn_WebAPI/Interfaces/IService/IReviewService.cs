using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> CreateReviewAsync(int userId, ReviewRequestDTO dto);
        Task<ReviewResponseDTO> UpdateReviewAsync(int reviewId, int userId, ReviewRequestDTO dto);
        Task<bool> DeleteReviewAsync(int reviewId, int userId);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByMenuItemAsync(int menuItemId);
        Task<double> GetAverageRatingForRestaurantAsync(int restaurantId);
        Task<double> GetAverageRatingForMenuItemAsync(int menuItemId);
        Task<IEnumerable<MenuItemRatingDTO>> GetTopRatedMenuItemsAsync(int top);
        Task<IEnumerable<RestaurantRatingDTO>> GetTopRatedRestaurantsAsync(int top);
    }
}
