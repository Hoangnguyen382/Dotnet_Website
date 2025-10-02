using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models.DTOs;
using DoAn_WebAPI.Interfaces.IRepository;

namespace DoAn_WebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        public CategoryService(ICategoryRepository categoryRepository, IRestaurantRepository restaurantRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;

        }
        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoryByRestaurantAsync(int restaurantId)
        {
            var categories = await _categoryRepository.GetAllCategoryByRestaurantAsync(restaurantId);
            return categories.Select(p => MapToCategoryReponseDTO(p)).ToList();
        }

        public async Task<CategoryResponseDTO> GetCategoryIdAsync(int id)
        {
            var item = await _categoryRepository.GetCategoryByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Menu item not found");

            return MapToCategoryReponseDTO(item);
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(int userId, int restaurantID, CategoryRequestDTO categoryRequestDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantID)
                throw new UnauthorizedAccessException("Không thể tạo danh mục cho nhà hàng khác.");

            var existingCategories = await _categoryRepository.GetAllCategoryByRestaurantAsync(restaurantID);
            if (existingCategories.Any(c => c.Name.Trim().ToLower() == categoryRequestDTO.Name.Trim().ToLower()))
                throw new InvalidOperationException("Tên danh mục đã tồn tại trong nhà hàng này.");
            var category = new Category
            {
                RestaurantID = restaurantID,
                Name = categoryRequestDTO.Name,
                Description = categoryRequestDTO.Description,
            };
            var created = await _categoryRepository.CreateCategoryAsync(category);
            return MapToCategoryReponseDTO(created);
        }

        public async Task<CategoryResponseDTO> UpdateCategoryAsync(int userId,int id, CategoryRequestDTO categoryRequestDTO)
        {
            var existing = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existing == null) return null;
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != existing.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền sửa danh mục này.");

            existing.Name = categoryRequestDTO.Name;
            existing.Description = categoryRequestDTO.Description;

            var updated = await _categoryRepository.UpdateCategoryAsync(existing);
            return updated != null ? MapToCategoryReponseDTO(updated) : null;
        }

        public async Task<bool> DeleteCategoryAsync(int id, int userId)
        {
            var existing = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existing == null) return false;
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != existing.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa danh mục này.");
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
        private CategoryResponseDTO MapToCategoryReponseDTO(Category category)
        {
            return new CategoryResponseDTO
            {
                CategoryID = category.CategoryID,
                RestaurantID = category.RestaurantID,
                Name = category.Name,
                Description = category.Description,
            };
        }

    }
}
