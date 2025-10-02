using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // Helper lấy userId từ token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItemAsync(int restaurantId, string? search, int categoryId = 0, int page = 1, int pageSize = 10)
        {
            var result = await _menuItemService.GetAllMenuItemAsync(restaurantId, search, categoryId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("restaurant/{restaurantId}")]
        [Authorize]
        public async Task<IActionResult> GetAllMenuByRestaurantAsync(int restaurantId, string? search, int categoryId = 0, int page = 1, int pageSize = 10)
        {
            int userId = GetUserIdFromToken();
            var result = await _menuItemService.GetAllMenuByRestaurantAsync(search, categoryId, page, pageSize, restaurantId, userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            int userId = GetUserIdFromToken();
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound();

            // Quyền sở hữu đã được service xử lý
            return Ok(menuItem);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateMenuItem(int restaurantId, [FromBody] MenuItemRequestDTO menuItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserIdFromToken();
            var createdItem = await _menuItemService.CreateMenuItemAsync(restaurantId, userId, menuItemDto);
            return Ok(createdItem);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemRequestDTO menuItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserIdFromToken();
            var updatedItem = await _menuItemService.UpdateMenuItemAsync(id, userId, menuItemDto);
            if (updatedItem == null)
                return NotFound();

            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            int userId = GetUserIdFromToken();
            var result = await _menuItemService.DeleteMenuItemAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
