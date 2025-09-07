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

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoryAsync()
        {
            var category = await _categoryRepository.GetAllCategoryAsync();
            // convert to DTO
            return category.Select(p => MapToCategoryReponseDTO(p)).ToList();
        }

        public async Task<CategoryResponseDTO> GetCategoryIdAsync(int id)
        {
            var item = await _categoryRepository.GetCategoryByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Menu item not found");

            return MapToCategoryReponseDTO(item);
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(int restaurantID, CategoryRequestDTO categoryRequestDTO)
        {
            var category = new Category
            {
                RestaurantID = restaurantID,
                Name = categoryRequestDTO.Name,
                Description = categoryRequestDTO.Description,
            };
            var created = await _categoryRepository.CreateCategoryAsync(category);
            return MapToCategoryReponseDTO(created);
        }

        public async Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryRequestDTO categoryRequestDTO)
        {
            var category = new Category
            {
                Name = categoryRequestDTO.Name,
                Description = categoryRequestDTO.Description,
            };
            var updated = await _categoryRepository.UpdateCategoryAsync(id, category);
            return updated != null ? MapToCategoryReponseDTO(updated) : null;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
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
