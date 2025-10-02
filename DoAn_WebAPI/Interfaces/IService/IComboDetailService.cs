using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IComboDetailService
    {
        // Admin
        Task<IEnumerable<ComboDetailResponseDTO>> GetDetailsByComboIdAsync(int comboId, int userId);
        Task<ComboDetailResponseDTO?> GetDetailByIdAsync(int id, int userId);
        Task<ComboDetailResponseDTO> AddDetailAsync(ComboDetailRequestDTO dto, int userId);
        Task<ComboDetailResponseDTO?> UpdateDetailAsync(int comboDetailId, ComboDetailRequestDTO dto, int userId);
        Task<bool> DeleteDetailAsync(int id, int userId);
        // Client
        Task<IEnumerable<ComboDetailResponseDTO>> GetAvailableDetailsByComboIdAsync(int comboId);
        Task<ComboDetailResponseDTO?> GetAvailableDetailByIdAsync(int id);
    }
}
