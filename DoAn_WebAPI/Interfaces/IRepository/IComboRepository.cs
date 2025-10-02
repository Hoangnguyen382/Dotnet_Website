using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IComboRepository
    {
        // Admin
        Task<IEnumerable<Combo>> GetCombosByRestaurantAsync(int restaurantId, int page, int pageSize);
        Task<Combo?> GetComboByIdAsync(int comboId);
        Task<Combo> CreateComboAsync(Combo combo);
        Task<Combo?> UpdateComboAsync(Combo combo);
        Task<bool> DeleteComboAsync(int comboId);
        // CLient 
        Task<IEnumerable<Combo>> GetAvailableCombosByRestaurantAsync(int restaurantId, int page, int pageSize);
        Task<Combo?> GetAvailableComboByIdAsync(int comboId);
        
    }
}