using Microsoft.AspNetCore.Mvc;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Google.Apis.Auth;
using DoAn_WebAPI.Interfaces.IRepository;

namespace DoAn_WebAPI.Controller
{    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService, IConfiguration configuration, IUserRepository userRepository)
        {
            _userService = userService;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            try
            {
                var result = await _userService.CreateUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginDTO dto)
        {
            var token = await _userService.LoginWithGoogleAsync(dto);
            if (token == null)
                return Unauthorized("Google login failed");

            return Ok(new { Token = token });
        }

        [HttpPut("update-role")]
        [Authorize (Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateUserRole(UserDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _userService.UpdateUserRoleAsync(dto, userId);
            if (result)
                return Ok("Cập nhật vai trò thành công.");

            return BadRequest("Cập nhật vai trò thất bại.");
        }
        [HttpGet("restaurant/{restaurantId}/staffs")]
        public async Task<IActionResult> GetStaffsByRestaurant(int restaurantId)
        {
            var staffs = await _userService.GetStaffsByRestaurantAsync(restaurantId);

            if (staffs == null || staffs.Count == 0)
                return NotFound("Không có nhân viên nào trong nhà hàng này.");

            return Ok(staffs);
        }
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            try
            {
                var result = await _userService.VerifyEmailAsync(token);
                if (result != null)
                {
                    return Ok("Email verified successfully");
                }
                else
                {
                    return BadRequest("Invalid token");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDTO);
                if (token == null)
                {
                    return BadRequest("Invalid email or password");
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassDTO forgotPassDTO)
        {
            try
            {
                var result = await _userService.ForgotPassword(forgotPassDTO.Email);
                if (result == null)
                {
                    return BadRequest("Email not found");
                }
                return Ok("Reset password link sent to your email");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassDTO resetPassDTO)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(resetPassDTO);
                if (result == null)
                {
                    return BadRequest("Invalid token or password");
                }
                return Ok("Password reset successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}