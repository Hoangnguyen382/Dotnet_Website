using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IReviewRepository
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId, int userId);
        Task<Review> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<IEnumerable<Review>> GetReviewsByMenuItemAsync(int menuItemId);
        Task<double> GetAverageRatingForRestaurantAsync(int restaurantId);
        Task<double> GetAverageRatingForMenuItemAsync(int menuItemId);
        Task<IEnumerable<MenuItemRatingDTO>> GetTopRatedMenuItemsAsync(int top);
        Task<IEnumerable<RestaurantRatingDTO>> GetTopRatedRestaurantsAsync(int top);
    }

}