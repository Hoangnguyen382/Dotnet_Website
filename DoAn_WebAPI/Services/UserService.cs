using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Cryptography;
using MimeKit.Encodings;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
using Google.Apis.Auth;
using System.Security.Cryptography;
namespace DoAn_WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<User?> CreateUserAsync(UserDTO user)
        {
            // check if user already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }
            // Kiểm tra độ mạnh mật khẩu
            if (!IsPasswordStrong(user.Password))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one special character.");
            }
            // mã hóa mật khẩu
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            // Create token to vrify account
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            // tạo link verify new account
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            var verifyLink = $"{baseUrl}/verify-email?token={token}";

            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = DateTime.UtcNow,
                VerificationToken = token,
                IsEmailVerified = false,
                Role = user.Role,
            };
            await _userRepository.CreateUserAsync(newUser);
            // send email
            var emailBody = $@"
             <h2>Hello {user.Name},</h2>
            <p>Thanks for registering. Please click the link below to verify your account:</p>
            <a href='{verifyLink}' style='padding:10px 20px;background:#007BFF;color:#fff;text-decoration:none;'>Verify Email</a>
            <p>If you didn’t request this, you can ignore this email.</p>";
            await _emailService.SendEmailAsync(user.Email, "Welcome", emailBody);
            return newUser;
        }
        public async Task<string?> LoginWithGoogleAsync(GoogleLoginDTO dto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);
                var user = await _userRepository.GetUserByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Name = payload.Name,
                        Email = payload.Email,
                        Password = null,
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true, 
                        Role = "User"
                    };
                    await _userRepository.CreateUserAsync(user);
                }
                var token = GenerateJwtToken(user);
                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var item = await _userRepository.GetUserByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Menu item not found");

            return item;
        }
        public async Task<User?> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetUserByVerificationTokenAsync(token);
            if (user == null) return null;
            user.IsEmailVerified = true;
            user.VerificationToken = null;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }


        // define token
        public string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:Key"];
            var formatKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(formatKey, SecurityAlgorithms.HmacSha256);

            // tạo claims
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("RestaurantID", user.RestaurantID.ToString() ?? "0"),
            };
            // tạo token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> UpdateUserRoleAsync(UserDTO dto, int updatedByUserId)
        {
            var updater = await _userRepository.GetUserByIdAsync(updatedByUserId);
            if (updater == null || (updater.Role != "Admin" && updater.Role != "Manager"))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật vai trò.");
            }
            if (dto.UserID == updatedByUserId)
            {
                throw new InvalidOperationException("Không thể cập nhật vai trò của chính mình.");
            }
            var userToUpdate = await _userRepository.GetUserByIdAsync(dto.UserID);
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng cần cập nhật.");
            }
            userToUpdate.Role = dto.Role;
            await _userRepository.UpdateUserAsync(userToUpdate);
            return true;
        }
        public async Task<List<User>> GetStaffsByRestaurantAsync(int restaurantId)
        {
            return await _userRepository.GetStaffsByRestaurantIdAsync(restaurantId);
        }
        public async Task<string?> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return null;
            }
            //check password
            // Hàm Verify có 2 tham số : password từ client và hash
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return null;
            }
            // check if email is verified
            if (!user.IsEmailVerified)
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        public async Task<User?> ForgotPassword(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
                return null;

            // tạo token random an toàn hơn
            var tokenBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(tokenBytes);
            var tokenResetPassword = Convert.ToBase64String(tokenBytes);

            // lưu token + expiry
            user.TokenResetPassword = tokenResetPassword;
            user.ExpiresTokenReset = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);

            // tạo link reset
            var baseUrl = _configuration["ClientURL:BaseUrl"];
            var resetLink = $"{baseUrl}/reset_password?token={Uri.EscapeDataString(tokenResetPassword)}";

            // gửi email
            var emailBody = $@"
                <h2>Đặt lại mật khẩu</h2>
                <p>Nhấn vào link bên dưới để đặt lại mật khẩu của bạn:</p>
                <a href='{resetLink}' style='padding:10px 20px;background:#007BFF;color:#fff;text-decoration:none;'>Reset Password</a>
                <p>Link sẽ hết hạn sau 1 giờ.</p>";

            await _emailService.SendEmailAsync(email, "Reset Password", emailBody);
            return user;
        }

        public async Task<User?> ResetPasswordAsync(ResetPassDTO resetPassDTO)
        {
            var user = await _userRepository.GetUserByResetTokenAsync(resetPassDTO.ResetToken);
            if (user == null)
            {
                return null;
            }
            if (!IsPasswordStrong(resetPassDTO.NewPassword))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one special character.");
            }
            if (user.ExpiresTokenReset < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Reset token has expired");
            }
            // mã hóa mật khẩu mới
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPassDTO.NewPassword);
            user.TokenResetPassword = null;
            user.ExpiresTokenReset = null;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasLowerCase && hasSpecialChar;
        }
    }
}