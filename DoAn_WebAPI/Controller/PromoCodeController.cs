using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using DoAn_WebAPI.Interfaces.IRepository;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;
        private readonly IRestaurantRepository _restaurantRepository;

        public PromoCodeController(IPromoCodeService promoCodeService, IRestaurantRepository restaurantRepository)
        {
            _promoCodeService = promoCodeService;
             _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCodeResponseDTO>>> GetAllPromoCodeAsync(int restaurantId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var list = await _promoCodeService.GetAllPromoCodesAsync(restaurantId, userId);
            return Ok(list);
        }
        // Public API cho client lấy danh sách mã giảm giá còn hiệu lực theo nhà hàng
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PromoCodeResponseDTO>>> GetPublicPromoCodes(int restaurantId)
        {
            var list = await _promoCodeService.GetPromoCodesByRestaurantAsync(restaurantId);
            return Ok(list);
        }

        [HttpGet("{id}")]
          [Authorize]
        public async Task<ActionResult<PromoCodeResponseDTO>> GetByIdPromoCodeAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim);
            var promoCode = await _promoCodeService.GetPromoCodeByIdAsync(id);
            if (promoCode == null)
                return NotFound();
            // 🔒 Kiểm tra quyền sở hữu
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(promoCode.RestaurantID);
            if (restaurant == null || restaurant.UserID != userId)
            {
                return Forbid("Bạn không có quyền truy cập món ăn này.");
            }
            return Ok(promoCode);
        }
        [HttpGet("validate-promo")]
        [Authorize]
        public async Task<IActionResult> ValidatePromo(string code, int restaurantId, decimal totalAmount, int totalQuantity)
        {
            try
            {
                var (discount, promoCodeId) = await _promoCodeService.ValidatePromoCodeAsync(code, restaurantId, totalAmount, totalQuantity);
                return Ok(new { discount, promoCodeId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
          [Authorize]
        public async Task<ActionResult<PromoCodeResponseDTO>> CreatePromoCodeAsync(int restaurantId, [FromBody] PromoCodeRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID not found in token.");
                }
                int userId = int.Parse(userIdClaim);
                var created = await _promoCodeService.CreatePromoCodeAsync(restaurantId, userId, dto);
                return Ok(created);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
          [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] PromoCodeRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var result = await _promoCodeService.UpdatePromoCodeAsync(id, userId, dto);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
          [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var result = await _promoCodeService.DeletePromoCodeAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
