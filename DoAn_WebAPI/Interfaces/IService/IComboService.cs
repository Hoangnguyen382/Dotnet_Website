using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IComboService
    {
        // Admin
        Task<IEnumerable<ComboResponseDTO>> GetCombosByRestaurantAsync(int restaurantId, int userId, int page, int pageSize);
        Task<ComboResponseDTO?> GetComboByIdAsync(int comboId, int userId);
        Task<ComboResponseDTO> CreateComboAsync(ComboRequestDTO dto, int userId, int restaurantId);
        Task<ComboResponseDTO?> UpdateComboAsync(int comboId, ComboRequestDTO dto, int userId);
        Task<bool> DeleteComboAsync(int comboId, int userId);
        // Client 
        Task<IEnumerable<ComboResponseDTO>> GetAvailableCombosByRestaurantAsync(int restaurantId, int page, int pageSize);
        Task<ComboResponseDTO?> GetAvailableComboByIdAsync(int comboId);
    }
}