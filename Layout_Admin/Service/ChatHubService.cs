using Microsoft.AspNetCore.SignalR.Client;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service
{
    public class ChatHubService
    {
        private readonly HubConnection _hubConnection;

        public event Action<MessageResponseDTO>? OnMessageReceived;

        public ChatHubService(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;

            _hubConnection.On<MessageResponseDTO>("ReceiveMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                await _hubConnection.StartAsync();
        }

        public async Task JoinConversation(int conversationId)
        {
            await _hubConnection.InvokeAsync("JoinConversation", conversationId);
        }

        public async Task LeaveConversation(int conversationId)
        {
            await _hubConnection.InvokeAsync("LeaveConversation", conversationId);
        }

        public async Task SendMessageAsync(CreateMessageRequestDTO dto)
        {
            await _hubConnection.InvokeAsync("SendMessageRealtime", dto);
        }
    }
}