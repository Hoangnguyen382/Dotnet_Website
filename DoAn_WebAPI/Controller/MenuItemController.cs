using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuItemController(IMenuItemService menuItemService, IRestaurantRepository restaurantRepository)
        {
            _menuItemService = menuItemService;
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItemAsync(int restaurantId, string? search, int categoryId, int page = 1, int pageSize = 10)
        {
            var result = await _menuItemService.GetAllMenuItemAsync(restaurantId, search, categoryId, page, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim);

            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            // 🔒 Kiểm tra quyền sở hữu
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(menuItem.RestaurantID);
            if (restaurant == null || restaurant.UserID != userId)
            {
                return Forbid("Bạn không có quyền truy cập món ăn này.");
            }

            return Ok(menuItem);
        }


        [HttpGet("restaurant/{RestaurantId}")]
        public async Task<IActionResult> GetAllMenuByRestaurantAsync(int RestaurantId, string? search, int categoryId, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var result = await _menuItemService.GetAllMenuByRestaurantAsync(search, categoryId, page, pageSize, RestaurantId, userId);
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateMenuItem(int restaurantID, [FromBody] MenuItemRequestDTO menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantID);
            if (restaurant == null)
            {
                return BadRequest("Restaurant not found for this user.");
            }
            // Gọi service để tạo menu item
            var createdItem = await _menuItemService.CreateMenuItemAsync(restaurantID, userId, menuItemDto);

            return Ok(createdItem);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemRequestDTO menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var updatedItem = await _menuItemService.UpdateMenuItemAsync(id, userId, menuItemDto);
            if (updatedItem == null)
            {
                return NotFound();
            }
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var result = await _menuItemService.DeleteMenuItemAsync(id, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
