using System.Net.Http.Json;
using Layout_Admin.Model.DTO;

namespace Layout_Admin.Service
{
    public class ChatService
    {
        private readonly AuthHttpClientFactory _factory;

        public ChatService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }
        public async Task<ConversationResponseDTO?> CreateConversationAsync(CreateConversationRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var res = await client.PostAsJsonAsync("api/Chat/conversation", dto);
            return await res.Content.ReadFromJsonAsync<ConversationResponseDTO>();
        }
        public async Task<ConversationResponseDTO?> GetConversationByOrderAsync(int orderId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<ConversationResponseDTO>($"api/Chat/conversation/order/{orderId}");
        }
        public async Task<IEnumerable<ConversationResponseDTO>?> GetConversationsByCustomerAsync(int customerId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<IEnumerable<ConversationResponseDTO>>($"api/Chat/conversations/customer/{customerId}");
        }
        public async Task<IEnumerable<ConversationResponseDTO>?> GetConversationsByRestaurantAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<IEnumerable<ConversationResponseDTO>>($"api/Chat/conversations/restaurant/{restaurantId}");
        }

        // Lấy danh sách messages
        public async Task<IEnumerable<MessageResponseDTO>?> GetMessagesAsync(int conversationId, int skip = 0, int take = 100)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<IEnumerable<MessageResponseDTO>>(
                $"api/Chat/messages/{conversationId}?skip={skip}&take={take}");
        }
        
        public async Task<MessageResponseDTO?> SendMessageAsync(CreateMessageRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var res = await client.PostAsJsonAsync("api/Chat/messages", dto);
            return await res.Content.ReadFromJsonAsync<MessageResponseDTO>();
        }

        public async Task<string?> UploadImageAsync(Stream fileStream, string fileName)
        {
            var client = await _factory.CreateClientAsync();
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "file", fileName);

            var res = await client.PostAsync("api/chat/upload", content);
            if (!res.IsSuccessStatusCode) return null;

            var result = await res.Content.ReadFromJsonAsync<UploadResult>();
            return result?.Url;
        }

        private class UploadResult
        {
            public string Url { get; set; } = "";
        }
    }
}
