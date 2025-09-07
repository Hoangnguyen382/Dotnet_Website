using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;


namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IUserService
    {
        Task<User?> CreateUserAsync(UserDTO user);
        Task<bool> UpdateUserRoleAsync(UserDTO dto, int updatedByUserId);
        Task<List<User>> GetStaffsByRestaurantAsync(int restaurantId);
        Task<User?> VerifyEmailAsync(string token);
        Task<User?> GetUserByIdAsync(int id);
        Task<string?> LoginAsync(LoginDTO loginDTO);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> ForgotPassword(string email);
        Task<User?> ResetPasswordAsync(ResetPassDTO resetPassDTO);
        string GenerateJwtToken(User user);
        Task<string?> LoginWithGoogleAsync(GoogleLoginDTO dto);
    }
}