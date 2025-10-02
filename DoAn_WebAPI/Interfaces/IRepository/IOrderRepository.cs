using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? date = null);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
    }
}