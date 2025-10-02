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
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository, IUserRepository userRepository)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
        }

        #region Admin
        public async Task<IEnumerable<RestaurantResponseDTO>> GetAllRestaurantAsync(string? search, int page, int pageSize)
        {
            var items = await _restaurantRepository.GetAllRestaurantsAsync();
            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(x => x.Name!.ToLower().Contains(search.ToLower()));
            }
            var pagedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedItems.Select(MapToRestaurantReponseDTO);
        }

        public async Task<IEnumerable<RestaurantResponseDTO>> GetAllRestaurantByUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User does not exist.");

            var restaurants = await _restaurantRepository.GetRestaurantsByUserIdAsync(userId);
            return restaurants.Select(MapToRestaurantReponseDTO);
        }

        public async Task<RestaurantResponseDTO?> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null) return null;
            return MapToRestaurantReponseDTO(restaurant);
        }

        public async Task<RestaurantResponseDTO> CreateRestaurantAsync(RestaurantRequestDTO dto, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User does not exist.");

            if (user.RestaurantID != null)
                throw new InvalidOperationException("Người dùng này đã có nhà hàng, không thể tạo thêm.");

            var restaurant = new Restaurant
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                OpeningHours = dto.OpeningHours,
                Description = dto.Description,
                IsActive = dto.IsActive,
                LogoUrl = dto.LogoUrl,
                UserID = userId,
            };

            var created = await _restaurantRepository.CreateRestaurantAsync(restaurant);

            // cập nhật lại User.RestaurantID sau khi tạo
            user.RestaurantID = created.RestaurantID;
            await _userRepository.UpdateUserAsync(user);

            return MapToRestaurantReponseDTO(created);
        }

        public async Task<RestaurantResponseDTO?> UpdateRestaurantAsync(int id, int userId, RestaurantRequestDTO dto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null) return null;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurant.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền sửa nhà hàng này.");

            restaurant.Name = dto.Name;
            restaurant.Address = dto.Address;
            restaurant.PhoneNumber = dto.PhoneNumber;
            restaurant.OpeningHours = dto.OpeningHours;
            restaurant.Description = dto.Description;
            restaurant.IsActive = dto.IsActive;
            restaurant.LogoUrl = dto.LogoUrl;

            var updated = await _restaurantRepository.UpdateRestaurantAsync(id, restaurant);
            return updated != null ? MapToRestaurantReponseDTO(updated) : null;
        }

        public async Task<bool> DeleteRestaurantAsync(int id, int userId)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null) return false;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurant.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa nhà hàng này.");

            return await _restaurantRepository.DeleteRestaurantAsync(id);
        }
        #endregion

        private RestaurantResponseDTO MapToRestaurantReponseDTO(Restaurant restaurant)
        {
            return new RestaurantResponseDTO
            {
                RestaurantID = restaurant.RestaurantID,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                OpeningHours = restaurant.OpeningHours,
                Description = restaurant.Description,
                IsActive = restaurant.IsActive,
                LogoUrl = restaurant.LogoUrl,
                UserID = restaurant.UserID
            };
        }

    }
}
