using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IDashboardService
    {
        Task<RevenueTodayDTO> GetTodayRevenueAsync(int restaurantId);
        Task<List<RevenueWeekDTO>> GetWeeklyRevenueAsync(int restaurantId);
        Task<OrderCountDTO> GetOrderCountTodayAsync(int restaurantId);
        Task<BestSellingItemDTO> GetBestSellingItemTodayAsync(int restaurantId);
        Task<List<TopSellingItemMonthlyDTO>> GetTopSellingItemMonthlyAsync(int restaurantId);
    }
}