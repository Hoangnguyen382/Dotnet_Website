using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId);
        Task<MenuItem?> GetMenuItemByIdAsync(int id);
        Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemtAsync(int id, MenuItem menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}