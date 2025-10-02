using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _resRepository;
        private readonly IUserRepository _userRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository, IRestaurantRepository resRepository, IUserRepository userRepository)
        {
            _menuItemRepository = menuItemRepository;
            _resRepository = resRepository;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuItemAsync(int restaurantId, string? search, int? categoryId, int page, int pageSize)
        {
            var allItems = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);

            if (!string.IsNullOrEmpty(search))
                allItems = allItems.Where(x => x.Name!.ToLower().Contains(search.ToLower()));

            if (categoryId.HasValue && categoryId.Value > 0)
                allItems = allItems.Where(x => x.CategoryID == categoryId.Value);

            var pagedItems = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedItems.Select(MapToMenuItemReponseDTO);
        }

        public async Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuByRestaurantAsync(string? search, int? categoryId, int page, int pageSize, int restaurantId, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantId)
                throw new UnauthorizedAccessException("Bạn không có quyền xem menu của nhà hàng này.");

            var items = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);

            if (!string.IsNullOrWhiteSpace(search))
                items = items.Where(x => x.Name!.ToLower().Contains(search.ToLower()));

            if (categoryId.HasValue && categoryId > 0)
                items = items.Where(x => x.CategoryID == categoryId.Value);

            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return pagedItems.Select(MapToMenuItemReponseDTO);
        }

        public async Task<MenuItemReponseDTO> GetMenuItemByIdAsync(int id)
        {
            var item = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Menu item not found");

            return MapToMenuItemReponseDTO(item);
        }

        public async Task<MenuItemReponseDTO> CreateMenuItemAsync(int restaurantID, int userId, MenuItemRequestDTO menuItemRequest)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền thêm món cho nhà hàng này.");

            // Kiểm tra trùng tên món
            var existingNames = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantID);
            if (existingNames.Any(x => x.Name!.Equals(menuItemRequest.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Tên món đã tồn tại trong nhà hàng.");

            var menuItem = new MenuItem
            {
                RestaurantID = restaurantID,
                CategoryID = menuItemRequest.CategoryID,
                Name = menuItemRequest.Name,
                Description = menuItemRequest.Description,
                Price = menuItemRequest.Price,
                DiscountPrice = menuItemRequest.DiscountPrice,
                SellingPrice = menuItemRequest.Price - (menuItemRequest.DiscountPrice ?? 0),
                IsAvailable = menuItemRequest.IsAvailable,
                ImageUrl = menuItemRequest.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _menuItemRepository.CreateMenuItemAsync(menuItem);
            return MapToMenuItemReponseDTO(created);
        }

        public async Task<MenuItemReponseDTO> UpdateMenuItemAsync(int id, int userId, MenuItemRequestDTO menuItemRequest)
        {
            var existing = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (existing == null)
                return null;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != existing.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền sửa món này.");

            var otherNames = await _menuItemRepository.GetMenuItemsByRestaurantAsync(existing.RestaurantID);
            if (otherNames.Any(x => x.MenuItemID != id && x.Name!.Equals(menuItemRequest.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Tên món đã tồn tại trong nhà hàng.");

            existing.Name = menuItemRequest.Name;
            existing.Description = menuItemRequest.Description;
            existing.Price = menuItemRequest.Price;
            existing.DiscountPrice = menuItemRequest.DiscountPrice;
            existing.SellingPrice = menuItemRequest.Price - (menuItemRequest.DiscountPrice ?? 0);
            existing.CategoryID = menuItemRequest.CategoryID;
            existing.IsAvailable = menuItemRequest.IsAvailable;
            existing.ImageUrl = menuItemRequest.ImageUrl;

            var updated = await _menuItemRepository.UpdateMenuItemtAsync(id, existing);
            return updated != null ? MapToMenuItemReponseDTO(updated) : null;
        }

        public async Task<bool> DeleteMenuItemAsync(int id, int userId)
        {
            var existing = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (existing == null) return false;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != existing.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa món này.");

            return await _menuItemRepository.DeleteMenuItemAsync(id);
        }

        private MenuItemReponseDTO MapToMenuItemReponseDTO(MenuItem menuItem)
        {
            return new MenuItemReponseDTO
            {
                MenuItemID = menuItem.MenuItemID,
                CategoryID = menuItem.CategoryID,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                DiscountPrice = menuItem.DiscountPrice,
                SellingPrice = menuItem.SellingPrice,
                IsAvailable = menuItem.IsAvailable,
                RestaurantID = menuItem.RestaurantID,
                CreatedAt = menuItem.CreatedAt,
                ImageUrl = menuItem.ImageUrl
            };
        }
    }
}
