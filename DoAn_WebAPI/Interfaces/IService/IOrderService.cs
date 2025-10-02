using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? date = null);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId);
        Task<OrderResponseDTO> GetOrderByIdAsync(int id);
        Task<OrderResponseDTO> CreateOrderAsync(int userId, int restaurantID, OrderRequestDTO dto, List<OrderDetailRequestDTO> orderDetails);
        Task<bool> UpdateOrderAsync(int id, OrderRequestDTO dto);
        Task<bool> DeleteOrderAsync(int id);
        Task MarkAsPaidAsync(string orderId);
        Task MarkAsFailedAsync(string orderId);
    }
}