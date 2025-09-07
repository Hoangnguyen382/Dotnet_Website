using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAn_WebAPI.Hubs;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public OrderController(IOrderService orderService, IRestaurantRepository restaurantRepository, IUserRepository userRepository, IHubContext<NotificationHub> hubContext)
        {
            _orderService = orderService;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
              _hubContext = hubContext;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrderByRestaurant(int restaurantId)
        {
            var list = await _orderService.GetOrdersByRestaurantIdAsync(restaurantId);
            return Ok(list);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrderByUser()
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
                var list = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(list);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Restaurant not found.");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        // Tạo order kèm chi tiết gửi riêng danh sách chi tiết trong body hoặc qua param list
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> Create(int restaurantID, [FromBody] OrderCreateWrapper dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantID);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found for this user.");
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim);
            var order = await _orderService.CreateOrderAsync(userId, restaurantID, dto.Order, dto.OrderDetails);
            // Gửi thông báo tới Admin
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", 
                        $"Bạn có đơn hàng mới #{order.OrderID} từ {restaurant.Name}");
            return CreatedAtAction(nameof(GetById), new { id = order.OrderID }, order);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var result = await _orderService.UpdateOrderAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        public class OrderCreateWrapper
        {
            [Required]
            public OrderRequestDTO Order { get; set; }
            [Required]
            public List<OrderDetailRequestDTO> OrderDetails { get; set; }
        }
    }
}