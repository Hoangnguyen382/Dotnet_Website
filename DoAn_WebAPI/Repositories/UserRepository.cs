using Microsoft.EntityFrameworkCore;
using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetUserByVerificationTokenAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return null;
            }
            user.IsEmailVerified = true;
            user.VerificationToken = null;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Email == email);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {   
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByResetTokenAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.TokenResetPassword == token);
            if (user == null )
            {
                return null;
            }
            if(DateTime.UtcNow > user.ExpiresTokenReset)
            {
                return null;
            }
            user.TokenResetPassword = null;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetStaffsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Users
            .Where(u => u.Role == "Staff" && u.RestaurantID == restaurantId)
            .ToListAsync();
        }
    }
}