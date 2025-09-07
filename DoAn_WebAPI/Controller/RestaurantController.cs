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
    
        // GET: api/Restaurant
        [HttpGet]

        public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetAllRestaurants(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _restaurantService.GetAllRestaurantAsync(search, page, pageSize);
            return Ok(result);
        }

        // GET: api/Restaurant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantResponseDTO>> GetRestaurantById(int id)
        {
            try
            {
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
                return Ok(restaurant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Restaurant not found.");
            }
        }

        [HttpGet("user")]
        [Authorize ]
        public async Task<ActionResult<RestaurantResponseDTO>> GetRestaurantByUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
                var restaurant = await _restaurantService.GetAllRestaurantByUserAsync(userId);
                return Ok(restaurant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Restaurant not found.");
            }
        }

        // POST: api/Restaurant
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RestaurantResponseDTO>> CreateRestaurant([FromBody] RestaurantRequestDTO restaurantRequest)
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

            var created = await _restaurantService.CreateRestaurantAsync(restaurantRequest, userId);
            return CreatedAtAction(nameof(GetRestaurantById), new { id = created.RestaurantID }, created);
        }

        // PUT: api/Restaurant/5
        [HttpPut("{id}")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<RestaurantResponseDTO>> UpdateRestaurant(int id, [FromBody] RestaurantRequestDTO restaurantRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            int userId = int.Parse(userIdClaim);

            try
            {
                var updated = await _restaurantService.UpdateRestaurantAsync(id, userId, restaurantRequest);
                if (updated == null)
                {
                    return NotFound("Restaurant not found or not owned by user");
                }
                return Ok(updated);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch
            {
                return BadRequest("Failed to update restaurant.");
            }
        }

        // DELETE: api/Restaurant/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim);

            var success = await _restaurantService.DeleteRestaurantAsync(id, userId);
            if (!success)
                return NotFound("Restaurant not found or not owned by user.");

            return NoContent();
        }
    }
}
