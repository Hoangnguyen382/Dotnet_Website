using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewID == reviewId && r.UserID == userId);
            if (review == null) return false;
            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.Include(r => r.User).FirstOrDefaultAsync(r => r.ReviewID == id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByRestaurantAsync(int restaurantId)
        {
            return await _context.Reviews.Include(r => r.User)
                .Where(r => r.RestaurantID == restaurantId)
                .OrderByDescending(r => r.ReviewDate).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByMenuItemAsync(int menuItemId)
        {
            return await _context.Reviews.Include(r => r.User)
                .Where(r => r.MenuItemID == menuItemId)
                .OrderByDescending(r => r.ReviewDate).ToListAsync();
        }

        public async Task<double> GetAverageRatingForRestaurantAsync(int restaurantId)
        {
            return await _context.Reviews.Where(r => r.RestaurantID == restaurantId).AverageAsync(r => (double?)r.Rating) ?? 0;
        }
        public async Task<double> GetAverageRatingForMenuItemAsync(int menuItemId)
        {
            return await _context.Reviews.Where(r => r.MenuItemID == menuItemId).AverageAsync(r => (double?)r.Rating) ?? 0;
        }
        public async Task<IEnumerable<MenuItemRatingDTO>> GetTopRatedMenuItemsAsync(int top)
        {
            return await _context.MenuItemRatingDTO
                .FromSqlRaw("EXEC GetTopRatedMenuItems @Top = {0}", top)
                .ToListAsync();
        }

        public async Task<IEnumerable<RestaurantRatingDTO>> GetTopRatedRestaurantsAsync(int top)
        {
            return await _context.RestaurantRatingDTO
                .FromSqlRaw("EXEC GetTopRatedRestaurants @Top = {0}", top)
                .ToListAsync();
        }

    }
}
