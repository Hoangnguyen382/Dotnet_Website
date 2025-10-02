using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DoAn_WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hub;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hub)
        {
            _chatService = chatService;
            _hub = hub;
        }

        [HttpPost("conversation")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequestDTO dto)
        {
            var conv = await _chatService.CreateConversationAsync(dto);
            return Ok(conv);
        }

        [HttpGet("conversation/order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var conv = await _chatService.CreateConversationAsync(new CreateConversationRequestDTO { OrderId = orderId, CustomerId = 0, RestaurantId = 0});
            return Ok(conv);
        }

        [HttpGet("conversations/customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var data = await _chatService.GetConversationsByCustomerAsync(customerId);
            return Ok(data);
        }

        [HttpGet("conversations/restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var data = await _chatService.GetConversationsByRestaurantAsync(restaurantId);
            return Ok(data);
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 100)
        {
            var messages = await _chatService.GetMessagesAsync(conversationId, skip, take);
            return Ok(messages);
        }

        [HttpPost("messages")]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageRequestDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            var result = await _chatService.SendMessageAsync(userId, dto);
            await _hub.Clients.Group($"conversation-{dto.ConversationId}").SendAsync("ReceiveMessage", result);

            return Ok(result);
        }

        [HttpPost("upload")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Empty file");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "chat");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/uploads/chat/{fileName}";
            return Ok(new { Url = url });
        }
    }
}