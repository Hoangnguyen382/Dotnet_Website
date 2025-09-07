using DoAn_WebAPI.Models;
namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<User?> CreateUserAsync(User user);
        Task<List<User>> GetStaffsByRestaurantIdAsync(int restaurantId);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByVerificationTokenAsync(string token);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> UpdateUserAsync(User user);
        Task<User?> GetUserByResetTokenAsync(string token);
    }
}