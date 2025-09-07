using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuItemAsync(int restaurantId,string? search, int? categoryId, int page, int pageSize);
        Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuByRestaurantAsync(string? search, int? categoryId, int page, int pageSize, int RestaurantId, int userId );
        Task<MenuItemReponseDTO> GetMenuItemByIdAsync(int id);
        Task<MenuItemReponseDTO> CreateMenuItemAsync(int restaurantID,int userId, MenuItemRequestDTO menuItemRequest);
        Task<MenuItemReponseDTO> UpdateMenuItemAsync(int id,int userId, MenuItemRequestDTO menuItemRequest);
        Task<bool> DeleteMenuItemAsync(int id,int userId);
    }
}
