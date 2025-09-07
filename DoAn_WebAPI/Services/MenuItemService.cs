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
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _resRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository, IRestaurantRepository resRepository)
        {
            _menuItemRepository = menuItemRepository;
            _resRepository = resRepository;
        }
        public async Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuItemAsync(int restaurantId, string? search, int? categoryId, int page, int pageSize)
        {
            var allItems = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId); // Lấy theo restaurant

            if (!string.IsNullOrEmpty(search))
            {
                allItems = allItems.Where(x => x.Name!.ToLower().Contains(search.ToLower()));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                allItems = allItems.Where(x => x.CategoryID == categoryId.Value);
            }

            var pagedItems = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedItems.Select(MapToMenuItemReponseDTO);
        }

        public async Task<IEnumerable<MenuItemReponseDTO>> GetAllMenuByRestaurantAsync(string? search, int? categoryId, int page, int pageSize, int restaurantId, int userId)
        {
            var restaurant = await _resRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found.");
            if (restaurant.UserID != userId)
                throw new UnauthorizedAccessException("You are not authorized to view menu items of this restaurant.");

            // Lấy danh sách menu items
            var item = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);

            if (!string.IsNullOrWhiteSpace(search))
                item = item.Where(x => x.Name.ToLower().Contains(search.ToLower()));

            if (categoryId.HasValue && categoryId > 0)
                item = item.Where(x => x.CategoryID == categoryId.Value);

            var pagedItems = item.Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

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
            var restaurant = await _resRepository.GetRestaurantByIdAsync(restaurantID);
            if (restaurant == null || restaurant.UserID != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to add items to this restaurant.");
            }
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
            var restaurant = await _resRepository.GetRestaurantByIdAsync(existing.RestaurantID);
            if (restaurant == null || restaurant.UserID != userId)
            {
                throw new UnauthorizedAccessException("You cannot edit this item");
            }
            var menuItem = new MenuItem
            {
                RestaurantID = existing.RestaurantID,
                CategoryID = menuItemRequest.CategoryID,
                Name = menuItemRequest.Name,
                Description = menuItemRequest.Description,
                SellingPrice = menuItemRequest.Price - (menuItemRequest.DiscountPrice ?? 0),
                Price = menuItemRequest.Price,
                DiscountPrice = menuItemRequest.DiscountPrice,
                IsAvailable = menuItemRequest.IsAvailable,
                ImageUrl = menuItemRequest.ImageUrl
            };
            var updated = await _menuItemRepository.UpdateMenuItemtAsync(id, menuItem);
            return updated != null ? MapToMenuItemReponseDTO(updated) : null;
        }

        public async Task<bool> DeleteMenuItemAsync(int id, int userId)
        {
            var existing = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (existing == null) return false;

            var restaurant = await _resRepository.GetRestaurantByIdAsync(existing.RestaurantID);
            if (restaurant == null || restaurant.UserID != userId)
            {
                throw new UnauthorizedAccessException("You cannot delete this item.");
            }
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
