using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using DoAn_WebAPI.Repositories;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IRestaurantRepository _restaurantRepository;

        public CategoryController(ICategoryService categoryService, IRestaurantRepository restaurantRepository)
        {
            _categoryService = categoryService;
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryAsync(int restaurantID)
        {
            var result = await _categoryService.GetAllCategoryByRestaurantAsync(restaurantID);
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateCategory(int restaurantID, [FromBody] CategoryRequestDTO categoryRequest)
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
                return NotFound("Restaurant not found.");
            }
            var createdItem = await _categoryService.CreateCategoryAsync(userId, restaurantID, categoryRequest);

            return Ok(createdItem);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDTO categoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            int userId = int.Parse(userIdClaim);
            var updatedCategory = await _categoryService.UpdateCategoryAsync(userId, id, categoryRequest);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            int userId = int.Parse(userIdClaim);
            var result = await _categoryService.DeleteCategoryAsync(id, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
