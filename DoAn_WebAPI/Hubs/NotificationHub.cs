using Microsoft.AspNetCore.SignalR;
namespace DoAn_WebAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinRestaurantGroup(string restaurantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"restaurant_{restaurantId}");
        }

        public async Task LeaveRestaurantGroup(string restaurantId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"restaurant_{restaurantId}");
        }
    }
}

