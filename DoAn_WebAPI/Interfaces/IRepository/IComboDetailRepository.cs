using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IComboDetailRepository
    {
        // Admin
        Task<IEnumerable<ComboDetail>> GetComboDetailsByComboIdAsync(int comboId);
        Task<ComboDetail?> GetComboDetailByIdAsync(int id);
        Task<ComboDetail> AddComboDetailAsync(ComboDetail detail);
        Task<ComboDetail?> UpdateComboDetailAsync(ComboDetail detail);
        Task<bool> DeleteComboDetailAsync(int id);
        // Client
        Task<IEnumerable<ComboDetail>> GetAvailableComboDetailsByComboIdAsync(int comboId);
        Task<ComboDetail?> GetAvailableComboDetailByIdAsync(int id);
        
    }
}
