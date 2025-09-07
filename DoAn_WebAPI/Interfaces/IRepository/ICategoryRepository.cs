using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}