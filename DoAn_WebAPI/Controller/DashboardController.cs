using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DoAn_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IUserRepository _userRepository;

        public DashboardController(IDashboardService dashboardService, IUserRepository userRepository)
        {
            _dashboardService = dashboardService;
            _userRepository = userRepository;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return int.Parse(userIdClaim);
        }

        private async Task<bool> UserHasAccessToRestaurant(int restaurantId, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user != null && user.RestaurantID == restaurantId;
        }

        [HttpGet("revenue/today/{restaurantId}")]
        public async Task<IActionResult> GetTodayRevenue(int restaurantId)
        {
            int userId = GetUserIdFromToken();

            if (!await UserHasAccessToRestaurant(restaurantId, userId))
                return Forbid("Bạn không có quyền truy cập doanh thu của nhà hàng này.");

            var revenue = await _dashboardService.GetTodayRevenueAsync(restaurantId);
            return Ok(revenue);
        }

        [HttpGet("revenue/week/{restaurantId}")]
        public async Task<IActionResult> GetWeeklyRevenue(int restaurantId)
        {
            int userId = GetUserIdFromToken();

            if (!await UserHasAccessToRestaurant(restaurantId, userId))
                return Forbid("Bạn không có quyền truy cập doanh thu của nhà hàng này.");

            var revenue = await _dashboardService.GetWeeklyRevenueAsync(restaurantId);
            return Ok(revenue);
        }

        [HttpGet("orders/today/{restaurantId}")]
        public async Task<IActionResult> GetOrderCountToday(int restaurantId)
        {
            int userId = GetUserIdFromToken();

            if (!await UserHasAccessToRestaurant(restaurantId, userId))
                return Forbid("Bạn không có quyền truy cập dữ liệu đơn hàng của nhà hàng này.");

            var count = await _dashboardService.GetOrderCountTodayAsync(restaurantId);
            return Ok(count);
        }

        [HttpGet("bestseller/today/{restaurantId}")]
        public async Task<IActionResult> GetBestSellingItemToday(int restaurantId)
        {
            int userId = GetUserIdFromToken();

            if (!await UserHasAccessToRestaurant(restaurantId, userId))
                return Forbid("Bạn không có quyền truy cập dữ liệu của nhà hàng này.");

            var item = await _dashboardService.GetBestSellingItemTodayAsync(restaurantId);
            if (item == null)
                return NoContent();

            return Ok(item);
        }

        [HttpGet("bestseller/month/{restaurantId}")]
        public async Task<IActionResult> GetTopSellingItemsMonthly(int restaurantId)
        {
            int userId = GetUserIdFromToken();

            if (!await UserHasAccessToRestaurant(restaurantId, userId))
                return Forbid("Bạn không có quyền truy cập dữ liệu của nhà hàng này.");

            var items = await _dashboardService.GetTopSellingItemMonthlyAsync(restaurantId);
            return Ok(items);
        }
    }
}
