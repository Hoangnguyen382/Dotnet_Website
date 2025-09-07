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
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var result = await _categoryService.GetAllCategoryAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCategory(int restaurantID, [FromBody] CategoryRequestDTO categoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            //       ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            // if (userIdClaim == null)
            // {
            //     return Unauthorized("User ID not found in token.");
            // }
            // int userId = int.Parse(userIdClaim);
            var category = await _restaurantRepository.GetRestaurantsByUserIdAsync(restaurantID);
            if (category == null)
            {
                return BadRequest("Restaurant not found.");
            }
            // Gọi service để tạo menu item
            var createdItem = await _categoryService.CreateCategoryAsync(restaurantID, categoryRequest);

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
            [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDTO categoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
            [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
