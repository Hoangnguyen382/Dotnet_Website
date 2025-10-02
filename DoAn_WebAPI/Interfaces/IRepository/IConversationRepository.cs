using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(int id);
    Task<Conversation?> GetByOrderIdAsync(int orderId);
    Task<IEnumerable<Conversation>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Conversation>> GetByRestaurantIdAsync(int restaurantId);
    Task<Conversation> CreateAsync(Conversation conversation);
}



}