using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ComboDetailsController : ControllerBase
    {
        private readonly IComboDetailService _comboDetailService;

        public ComboDetailsController(IComboDetailService comboDetailService)
        {
            _comboDetailService = comboDetailService;
        }
        #region Admin
        [HttpGet("combo/{comboId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ComboDetailResponseDTO>>> GetByComboId(int comboId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var details = await _comboDetailService.GetDetailsByComboIdAsync(comboId, userId);
                return Ok(details);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ComboDetailResponseDTO>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            var detail = await _comboDetailService.GetDetailByIdAsync(id, userId);
            if (detail == null) return NotFound();
            return Ok(detail);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ComboDetailResponseDTO>> Create([FromBody] ComboDetailRequestDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var created = await _comboDetailService.AddDetailAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = created.ComboDetailID }, created);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ComboDetailResponseDTO>> Update(int id, [FromBody] ComboDetailRequestDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var updated = await _comboDetailService.UpdateDetailAsync(id, dto, userId);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            try
            {
                var deleted = await _comboDetailService.DeleteDetailAsync(id, userId);
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
        [HttpGet("public/combo/{comboId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ComboDetailResponseDTO>>> GetPublicByComboId(int comboId)
        {
            var details = await _comboDetailService.GetAvailableDetailsByComboIdAsync(comboId);
            return Ok(details);
        }
        [HttpGet("public/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ComboDetailResponseDTO>> GetPublicById(int id)
        {
            var detail = await _comboDetailService.GetAvailableDetailByIdAsync(id);
            if (detail == null) return NotFound();
            return Ok(detail);
        }
        #endregion
    }
}
