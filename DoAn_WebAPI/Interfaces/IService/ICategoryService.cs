using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoryAsync();
        Task<CategoryResponseDTO> GetCategoryIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(int restaurantID, CategoryRequestDTO categoryRequestDTO);
        Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryRequestDTO categoryRequestDTO);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
