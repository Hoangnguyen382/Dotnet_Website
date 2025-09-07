using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
  public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<OrderDetail?> GetOrderDetailByIdAsync(int id);
        Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}