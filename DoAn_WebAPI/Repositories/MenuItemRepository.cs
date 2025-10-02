using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DoAn_WebAPI.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
        {
            return await _context.MenuItems.Where(m =>m.RestaurantID == restaurantId).ToListAsync();
            
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }
        public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
        {
            menuItem.CreatedAt = DateTime.Now;
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemtAsync(int id, MenuItem menuItem)
        {
            // kiểm tra xem sản phẩm có tồn tại không
            var existingMenuItem = await _context.MenuItems.FindAsync(id);
            // nếu không tồn tại thì trả về null
            if (existingMenuItem == null)
            {
                return null;
            }
            // nếu có tồn tại thì cập nhật thông tin
            existingMenuItem.Name = menuItem.Name;
            existingMenuItem.Description = menuItem.Description;
            existingMenuItem.Price = menuItem.Price;
            existingMenuItem.DiscountPrice = menuItem.DiscountPrice;
            existingMenuItem.IsAvailable = menuItem.IsAvailable;
            existingMenuItem.CategoryID = menuItem.CategoryID;
            // cập nhật vào database
            _context.MenuItems.Update(existingMenuItem);
            await _context.SaveChangesAsync();
            return existingMenuItem;
        }
        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var existingMenuItem = await _context.MenuItems.FindAsync(id);
            if (existingMenuItem == null)
            {
                return false;
            }
            _context.MenuItems.Remove(existingMenuItem);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}