using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return int.Parse(userIdClaim);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PromoCodeResponseDTO>>> GetAllPromoCodeAsync(int restaurantId)
        {
            int userId = GetUserIdFromToken();
            var list = await _promoCodeService.GetAllPromoCodesAsync(restaurantId, userId);
            return Ok(list);
        }

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
            int userId = GetUserIdFromToken();
            var promoCode = await _promoCodeService.GetPromoCodeByIdAsync(id);
            if (promoCode == null)
                return NotFound();

            return Ok(promoCode); 
        }

        [HttpGet("validate-promo")]
        [Authorize]
        public async Task<IActionResult> ValidatePromo([FromQuery] string code, [FromQuery] int restaurantId, [FromQuery] decimal totalAmount, [FromQuery] int totalQuantity)
        {
            var result = await _promoCodeService.ValidatePromoCodeAsync(code, restaurantId, totalAmount, totalQuantity);
            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PromoCodeResponseDTO>> CreatePromoCodeAsync(int restaurantId, [FromBody] PromoCodeRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserIdFromToken();
            try
            {
                var created = await _promoCodeService.CreatePromoCodeAsync(userId, restaurantId, dto);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] PromoCodeRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserIdFromToken();
            var result = await _promoCodeService.UpdatePromoCodeAsync(id, userId, dto);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetUserIdFromToken();
            var result = await _promoCodeService.DeletePromoCodeAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
