using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IChatService
    {
        Task<ConversationResponseDTO> CreateConversationAsync(CreateConversationRequestDTO dto);
        Task<ConversationResponseDTO?> GetConversationByOrderAsync(int orderId);
        Task<IEnumerable<ConversationResponseDTO>> GetConversationsByCustomerAsync(int customerId);
        Task<IEnumerable<ConversationResponseDTO>> GetConversationsByRestaurantAsync(int restaurantId);
        Task<MessageResponseDTO> SendMessageAsync(int userId, CreateMessageRequestDTO dto);
        Task<IEnumerable<MessageResponseDTO>> GetMessagesAsync(int conversationId, int skip = 0, int take = 100);
    }

}