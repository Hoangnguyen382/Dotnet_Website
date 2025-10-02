using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(conversationId));
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(conversationId));
    }
    public async Task SendMessageRealtime(CreateMessageRequestDTO dto)
    {
        var userIdStr = Context.User?.FindFirst("UserId")?.Value 
                 ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            throw new HubException("Invalid user");

        var saved = await _chatService.SendMessageAsync(userId, dto);

        await Clients.Group(GetGroupName(dto.ConversationId))
                     .SendAsync("ReceiveMessage", saved);
    }

    private string GetGroupName(int convId) => $"conversation-{convId}";
}
