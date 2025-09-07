// Services/DashboardService.cs
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<RevenueTodayDTO> GetTodayRevenueAsync(int restaurantId)
        {
            return await _dashboardRepository.GetTodayRevenueAsync(restaurantId);
        }

        public async Task<List<RevenueWeekDTO>> GetWeeklyRevenueAsync(int restaurantId)
        {
            return await _dashboardRepository.GetWeeklyRevenueAsync(restaurantId);
        }

        public async Task<OrderCountDTO> GetOrderCountTodayAsync(int restaurantId)
        {
            return await _dashboardRepository.GetOrderCountTodayAsync(restaurantId);
        }

        public async Task<BestSellingItemDTO> GetBestSellingItemTodayAsync(int restaurantId)
        {
            return await _dashboardRepository.GetBestSellingItemTodayAsync(restaurantId);
        }

        public async Task<List<TopSellingItemMonthlyDTO>> GetTopSellingItemMonthlyAsync(int restaurantId)
        {
            return await _dashboardRepository.GetTopSellingItemMonthlyAsync(restaurantId);
        }
    }
}
