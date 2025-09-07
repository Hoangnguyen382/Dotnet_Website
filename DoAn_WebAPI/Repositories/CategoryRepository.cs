using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DoAn_WebAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            // tạo data cho created_at, updated_at
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
       
        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            // kiểm tra xem sản phẩm có tồn tại không
            var existingCategory = await _context.Categories.FindAsync(id);
            // nếu không tồn tại thì trả về null
            if (existingCategory == null)
            {
                return null;
            }
            // nếu có tồn tại thì cập nhật thông tin
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            // cập nhật vào database
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existingCategory = await _context.Categories.FindAsync(id);    
            if(existingCategory == null)
            {
                return false;
            }
            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}