using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<OrderDetailResponseDTO> GetOrderDetailByIdAsync(int id);
        Task<OrderDetailResponseDTO> CreateOrderDetailAsync(OrderDetailRequestDTO dto);
        Task<bool> UpdateOrderDetailAsync(int id, OrderDetailRequestDTO dto);
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}
