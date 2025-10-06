namespace DoAn_WebAPI.Interfaces.IRepository
{
    using DoAn_WebAPI.Models;
    public interface IMessageImageRepository
    {
        Task<MessageImage> CreateAsync(MessageImage image);
        Task<List<MessageImage>> GetByMessageIdAsync(int messageId);
    }
}