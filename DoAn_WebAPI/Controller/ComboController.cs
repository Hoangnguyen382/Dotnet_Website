using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_WebAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombosController : ControllerBase
    {
        private readonly IComboService _comboService;

        public CombosController(IComboService comboService)
        {
            _comboService = comboService;
        }
        #region Admin
        [HttpGet("restaurant/{restaurantId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ComboResponseDTO>>> GetCombosByRestaurant(int restaurantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            try
            {
                var combos = await _comboService.GetCombosByRestaurantAsync(restaurantId, userId, page, pageSize);
                return Ok(combos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("{comboId}")]
        [Authorize]
        public async Task<ActionResult<ComboResponseDTO>> GetComboById(int comboId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            var combo = await _comboService.GetComboByIdAsync(comboId, userId);
            if (combo == null) return NotFound();
            return Ok(combo);
        }

        [HttpPost("restaurant/{restaurantId}")]
        [Authorize]
        public async Task<ActionResult<ComboResponseDTO>> CreateCombo(int restaurantId, [FromBody] ComboRequestDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            var created = await _comboService.CreateComboAsync(dto, userId, restaurantId);
            return CreatedAtAction(nameof(GetComboById), new { comboId = created.ComboID }, created);
        }

        [HttpPut("{comboId}")]
        [Authorize]
        public async Task<ActionResult<ComboResponseDTO>> UpdateCombo(int comboId, [FromBody] ComboRequestDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var updated = await _comboService.UpdateComboAsync(comboId, dto, userId);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpDelete("{comboId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCombo(int comboId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var deleted = await _comboService.DeleteComboAsync(comboId, userId);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
        #endregion

        #region Client
        [AllowAnonymous]
        [HttpGet("public/restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<ComboResponseDTO>>> GetPublicCombosByRestaurant(int restaurantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var combos = await _comboService.GetAvailableCombosByRestaurantAsync(restaurantId, page, pageSize);
            return Ok(combos);
        }
        
        [AllowAnonymous]
        [HttpGet("public/{comboId}")]
        public async Task<ActionResult<ComboResponseDTO>> GetPublicComboById(int comboId)
        {
            var combo = await _comboService.GetAvailableComboByIdAsync(comboId);
            if (combo == null) return NotFound();
            return Ok(combo);
        }

        #endregion
    }
}