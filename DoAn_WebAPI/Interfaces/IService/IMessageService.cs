using DoAn_WebAPI.Models.DTOs;
namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IMessageService
    {
        Task<MessageResponseDTO> SendMessageAsync(CreateMessageRequestDTO messageRequest);
        Task<List<MessageResponseDTO>> GetMessagesByConversationIdAsync(int conversationId);
    }
}