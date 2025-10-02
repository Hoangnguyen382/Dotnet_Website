using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using DoAn_WebAPI.Models.DTOs;
using DoAn_WebAPI.Interfaces.IService;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetAllRestaurants(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _restaurantService.GetAllRestaurantAsync(search, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantResponseDTO>> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound("Restaurant not found.");
            return Ok(restaurant);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetRestaurantByUser()
        {
            int userId = GetUserIdFromToken();
            var restaurants = await _restaurantService.GetAllRestaurantByUserAsync(userId);
            return Ok(restaurants);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RestaurantResponseDTO>> CreateRestaurant([FromBody] RestaurantRequestDTO restaurantRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserIdFromToken();
            var created = await _restaurantService.CreateRestaurantAsync(restaurantRequest, userId);
            return CreatedAtAction(nameof(GetRestaurantById), new { id = created.RestaurantID }, created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<RestaurantResponseDTO>> UpdateRestaurant(int id, [FromBody] RestaurantRequestDTO restaurantRequest)
        {
            int userId = GetUserIdFromToken();
            var updated = await _restaurantService.UpdateRestaurantAsync(id, userId, restaurantRequest);

            if (updated == null)
                return NotFound("Restaurant not found.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            int userId = GetUserIdFromToken();
            var success = await _restaurantService.DeleteRestaurantAsync(id, userId);

            if (!success)
                return NotFound("Restaurant not found.");

            return NoContent();
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");

            return int.Parse(userIdClaim);
        }
    }
}
