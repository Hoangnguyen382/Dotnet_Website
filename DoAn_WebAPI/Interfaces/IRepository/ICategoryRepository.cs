using DoAn_WebAPI.Models;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoryByRestaurantAsync(int restaurantId);
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);
}
