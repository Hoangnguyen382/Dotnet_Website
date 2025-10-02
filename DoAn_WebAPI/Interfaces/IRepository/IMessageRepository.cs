using DoAn_WebAPI.Models;
namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IMessageRepository
    {
        Task<Message> CreateAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(int conversationId, int skip = 0, int take = 50);
    }
}