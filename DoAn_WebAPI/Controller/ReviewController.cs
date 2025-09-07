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
        public class ReviewsController : ControllerBase
        {
            private readonly IReviewService _reviewService;

            public ReviewsController(IReviewService reviewService)
            {
                _reviewService = reviewService;
            }
            // Review nhà hàng
            [HttpPost("restaurant")]
            [Authorize]
            public async Task<IActionResult> CreateRestaurantReview([FromBody] ReviewRequestDTO dto)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                dto.MenuItemID = null; // ép null
                var result = await _reviewService.CreateReviewAsync(userId, dto);
                return Ok(result);
            }
            // Review món ăn
            [HttpPost("menuitem")]
            [Authorize]
            public async Task<IActionResult> CreateMenuItemReview([FromBody] ReviewRequestDTO dto)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (!dto.MenuItemID.HasValue)
                    return BadRequest("MenuItemID là bắt buộc khi đánh giá món ăn.");
                var result = await _reviewService.CreateReviewAsync(userId, dto);
                return Ok(result);
            }

            [HttpPut("{reviewId}/{userId}")]
            public async Task<IActionResult> UpdateReview(int reviewId, int userId, [FromBody] ReviewRequestDTO dto)
            {
                var result = await _reviewService.UpdateReviewAsync(reviewId, userId, dto);
                return Ok(result);
            }

            [HttpDelete("{reviewId}/{userId}")]
            public async Task<IActionResult> DeleteReview(int reviewId, int userId)
            {
                var success = await _reviewService.DeleteReviewAsync(reviewId, userId);
                if (!success) return NotFound();
                return Ok();
            }

            [HttpGet("restaurant/{restaurantId}")]
            public async Task<IActionResult> GetReviewsByRestaurant(int restaurantId)
            {
                var list = await _reviewService.GetReviewsByRestaurantAsync(restaurantId);
                return Ok(list);
            }

            [HttpGet("menuitem/{menuItemId}")]
            public async Task<IActionResult> GetReviewsByMenuItem(int menuItemId)
            {
                var list = await _reviewService.GetReviewsByMenuItemAsync(menuItemId);
                return Ok(list);
            }

            [HttpGet("average/restaurant/{restaurantId}")]
            public async Task<IActionResult> GetAvgRatingRestaurant(int restaurantId)
                => Ok(await _reviewService.GetAverageRatingForRestaurantAsync(restaurantId));

            [HttpGet("average/menuitem/{menuItemId}")]
            public async Task<IActionResult> GetAvgRatingMenuItem(int menuItemId)
                => Ok(await _reviewService.GetAverageRatingForMenuItemAsync(menuItemId));

            [HttpGet("top-menuitems")]
            public async Task<IActionResult> GetTopRatedMenuItems([FromQuery] int top = 10)
            {
                var list = await _reviewService.GetTopRatedMenuItemsAsync(top);
                return Ok(list);
            }
            [HttpGet("top-restaurants")]
            public async Task<IActionResult> GetTopRatedRestaurants([FromQuery] int top = 10)
            {
                var list = await _reviewService.GetTopRatedRestaurantsAsync(top);
                return Ok(list);
            }
    }

    }
