using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DoAn_WebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? date = null)
        {
            var targetDate = date?.Date ?? DateTime.Today;

            return await _context.Orders
                .Where(o => o.RestaurantID == restaurantId && o.OrderDate.Date == targetDate)
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders.Where(o => o.UserID == userId)
                                        .OrderByDescending(o => o.OrderDate)
                                        .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                        .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}