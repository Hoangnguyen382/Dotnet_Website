using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using DoAn_WebAPI.Interfaces.IRepository;

namespace DoAn_WebAPI.Services
{
   public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepo;
    private readonly IUserRepository _userRepo;

    public ReviewService(IReviewRepository reviewRepo, IUserRepository userRepo)
    {
        _reviewRepo = reviewRepo;
        _userRepo = userRepo;
    }

    public async Task<ReviewResponseDTO> CreateReviewAsync(int userId, ReviewRequestDTO dto)
    {
        var user = await _userRepo.GetUserByIdAsync(userId);
        if (dto.MenuItemID.HasValue)
        {
            var review = new Review
            {
                UserID = userId,
                RestaurantID = dto.RestaurantID ?? 0,
                MenuItemID = dto.MenuItemID,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = DateTime.Now
            };
            var created = await _reviewRepo.CreateReviewAsync(review);
            return MapToReviewResponseDTO(created, user);
        }
        else
        {
            var review = new Review
            {
                UserID = userId,
                RestaurantID = dto.RestaurantID ?? 0,
                MenuItemID = null, 
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = DateTime.Now
            };
            var created = await _reviewRepo.CreateReviewAsync(review);
            return MapToReviewResponseDTO(created, user);
        }
    }


    public async Task<ReviewResponseDTO> UpdateReviewAsync(int reviewId, int userId, ReviewRequestDTO dto)
    {
        var existing = await _reviewRepo.GetReviewByIdAsync(reviewId);
        if (existing == null || existing.UserID != userId)
            throw new UnauthorizedAccessException("Không thể chỉnh sửa đánh giá người khác.");

        existing.Rating = dto.Rating;
        existing.Comment = dto.Comment;
        var updated = await _reviewRepo.UpdateReviewAsync(existing);
        return MapToReviewResponseDTO(updated, existing.User);
    }

    public async Task<bool> DeleteReviewAsync(int reviewId, int userId)
        => await _reviewRepo.DeleteReviewAsync(reviewId, userId);

    public async Task<IEnumerable<ReviewResponseDTO>> GetReviewsByRestaurantAsync(int restaurantId)
    {
        var reviews = await _reviewRepo.GetReviewsByRestaurantAsync(restaurantId);
        return reviews.Select(r => MapToReviewResponseDTO(r, r.User));
    }

    public async Task<IEnumerable<ReviewResponseDTO>> GetReviewsByMenuItemAsync(int menuItemId)
    {
        var reviews = await _reviewRepo.GetReviewsByMenuItemAsync(menuItemId);
        return reviews.Select(r => MapToReviewResponseDTO(r, r.User));
    }

    public Task<double> GetAverageRatingForRestaurantAsync(int restaurantId)
        => _reviewRepo.GetAverageRatingForRestaurantAsync(restaurantId);

    public Task<double> GetAverageRatingForMenuItemAsync(int menuItemId)
        => _reviewRepo.GetAverageRatingForMenuItemAsync(menuItemId);
    public async Task<IEnumerable<MenuItemRatingDTO>> GetTopRatedMenuItemsAsync(int top)
    {
        return await _reviewRepo.GetTopRatedMenuItemsAsync(top); 
    }
    public async Task<IEnumerable<RestaurantRatingDTO>> GetTopRatedRestaurantsAsync(int top)
    {
        return await _reviewRepo.GetTopRatedRestaurantsAsync(top);
    }

    private ReviewResponseDTO MapToReviewResponseDTO(Review r, User u) => new()
        {
            ReviewID = r.ReviewID,
            RestaurantID = r.RestaurantID,
            MenuItemID = r.MenuItemID,
            Rating = r.Rating,
            Comment = r.Comment,
            ReviewDate = r.ReviewDate,
            UserID = u.UserID,
            UserName = u.Name
        };
}
 
}
