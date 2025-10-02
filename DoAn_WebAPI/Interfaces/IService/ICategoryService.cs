using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoryByRestaurantAsync(int restaurantId);
        Task<CategoryResponseDTO> GetCategoryIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(int userId, int restaurantID, CategoryRequestDTO categoryRequestDTO);
        Task<CategoryResponseDTO> UpdateCategoryAsync(int userId, int id, CategoryRequestDTO categoryRequestDTO);
        Task<bool> DeleteCategoryAsync(int userId, int id);
    }
}
