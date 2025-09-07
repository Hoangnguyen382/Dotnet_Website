using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DoAn_WebAPI.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
          public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderID == orderId)
                                             .ToListAsync();
        }
        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailID == id);
        }
        public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }
        public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
             _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }
        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return false;
            }
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}