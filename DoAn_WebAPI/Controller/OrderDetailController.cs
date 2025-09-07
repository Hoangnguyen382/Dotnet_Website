using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/orderdetails")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailResponseDTO>>> GetByOrderId(int orderId)
        {
            var list = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailResponseDTO>> GetById(int id)
        {
            var detail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (detail == null) return NotFound();
            return Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailResponseDTO>> Create([FromBody] OrderDetailRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _orderDetailService.CreateOrderDetailAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderDetailID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetailRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _orderDetailService.UpdateOrderDetailAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}