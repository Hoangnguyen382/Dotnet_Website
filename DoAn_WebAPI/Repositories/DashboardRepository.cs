// Repository/DashboardRepository.cs
using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RevenueTodayDTO> GetTodayRevenueAsync(int restaurantId)
        {
            var result = await _context.RevenueTodayResults
                .FromSqlRaw("EXEC sp_GetTodayRevenue @RestaurantId", new SqlParameter("@RestaurantId", restaurantId))
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<List<RevenueWeekDTO>> GetWeeklyRevenueAsync(int restaurantId)
        {
            var result = await _context.Set<RevenueWeekDTO>()
            .FromSqlRaw("EXEC sp_GetWeeklyRevenue @RestaurantId", new SqlParameter("@RestaurantId", restaurantId))
            .ToListAsync();
        return result;
        }

        public async Task<OrderCountDTO> GetOrderCountTodayAsync(int restaurantId)
        {
            var result = await _context.OrderCountResults
                .FromSqlRaw("EXEC sp_GetOrderCountToday @RestaurantId", new SqlParameter("@RestaurantId", restaurantId))
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<BestSellingItemDTO> GetBestSellingItemTodayAsync(int restaurantId)
        {
            var result = await _context.BestSellingItemResults
                .FromSqlRaw("EXEC sp_GetBestSellingItemToday @RestaurantId", new SqlParameter("@RestaurantId", restaurantId))
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<List<TopSellingItemMonthlyDTO>> GetTopSellingItemMonthlyAsync(int restaurantId)
        {
            var result = await _context.TopSellingItemMonthlyResults
                .FromSqlRaw("EXEC sp_GetTop5BestSellingItemsThisMonth @RestaurantId", new SqlParameter("@RestaurantId", restaurantId))
                .ToListAsync();
            return result;
        }


    }
}
