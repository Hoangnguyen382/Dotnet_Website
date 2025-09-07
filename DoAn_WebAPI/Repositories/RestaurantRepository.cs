    using DoAn_WebAPI.Data;
    using DoAn_WebAPI.Interfaces.IRepository;
    using DoAn_WebAPI.Models;
    using Microsoft.EntityFrameworkCore;

    namespace DoAn_WebAPI.Repositories
    {
        public class RestaurantRepository : IRestaurantRepository
        {
            private readonly ApplicationDbContext _context;

            public RestaurantRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
            {
                return await _context.Restaurants.ToListAsync();
            }
            public async Task<IEnumerable<Restaurant>> GetRestaurantsByUserIdAsync(int userId)
            {
                return await _context.Restaurants.Where(r => r.UserID == userId).ToListAsync();
            }

            public async Task<Restaurant> GetRestaurantByIdAsync(int id)
            {
                return await _context.Restaurants.FindAsync(id);
            }
            public async Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant)
            {
                // tạo data cho created_at, updated_at
                restaurant.CreatedAt = DateTime.UtcNow;
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                return restaurant;
            }

            public async Task<Restaurant> UpdateRestaurantAsync(int id, Restaurant restaurant)
            {
                // kiểm tra xem sản phẩm có tồn tại không
                var existingRestaurant = await _context.Restaurants.FindAsync(id);
                // nếu không tồn tại thì trả về null
                if (existingRestaurant == null)
                {
                    return null;
                }
                // nếu có tồn tại thì cập nhật thông tin
                existingRestaurant.Name = restaurant.Name;
                existingRestaurant.Address = restaurant.Address;
                existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
                existingRestaurant.Description = restaurant.Description;
                existingRestaurant.OpeningHours = restaurant.OpeningHours;
                existingRestaurant.IsActive = restaurant.IsActive;
                existingRestaurant.LogoUrl = restaurant.LogoUrl;
                // cập nhật vào database
                _context.Restaurants.Update(existingRestaurant);
                await _context.SaveChangesAsync();
                return existingRestaurant;
            }
            public async Task<bool> DeleteRestaurantAsync(int id)
            {
                var existingRestaurant = await _context.Restaurants.FindAsync(id);
                if (existingRestaurant == null)
                {
                    return false;
                }
                _context.Restaurants.Remove(existingRestaurant);
                await _context.SaveChangesAsync();
                return true;
            }

        }
    }