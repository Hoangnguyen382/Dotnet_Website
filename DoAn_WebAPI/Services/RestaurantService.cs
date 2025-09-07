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
            var item = await _restaurantRepository.GetRestaurantsByUserIdAsync(userId);
            return item.Select(MapToRestaurantReponseDTO);
        }

        public async Task<RestaurantResponseDTO> GetRestaurantByIdAsync(int id)
        {
            var item = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Menu item not found");

                return MapToRestaurantReponseDTO(item);
        }
        public async Task<RestaurantResponseDTO> CreateRestaurantAsync(RestaurantRequestDTO restaurantRequestDTO, int userId)
        {
            // check if user already has a restaurant
            var userRestaurants = await _restaurantRepository.GetRestaurantsByUserIdAsync(userId);
            if (userRestaurants.Any())
            {
                throw new InvalidOperationException("This user already has a restaurant and cannot create another one.");
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User does not exist.");
            }
            var restaurant = new Restaurant
            {
                Name = restaurantRequestDTO.Name,
                Address = restaurantRequestDTO.Address,
                PhoneNumber = restaurantRequestDTO.PhoneNumber,
                OpeningHours = restaurantRequestDTO.OpeningHours,
                Description = restaurantRequestDTO.Description,
                IsActive = restaurantRequestDTO.IsActive,
                LogoUrl = restaurantRequestDTO.LogoUrl,
                UserID = userId,
            };
            var created = await _restaurantRepository.CreateRestaurantAsync(restaurant);
            return MapToRestaurantReponseDTO(created);
        }
        public async Task<RestaurantResponseDTO> UpdateRestaurantAsync(int id,int userId, RestaurantRequestDTO restaurantRequestDTO)
        {
            var existingRestaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (existingRestaurant == null)
                return null;
            if (existingRestaurant.UserID != userId)
                throw new UnauthorizedAccessException("You do not have permission to update this restaurant.");
            var restaurant = new Restaurant
            {
                Name = restaurantRequestDTO.Name,
                Address = restaurantRequestDTO.Address,
                PhoneNumber = restaurantRequestDTO.PhoneNumber,
                OpeningHours = restaurantRequestDTO.OpeningHours,
                Description = restaurantRequestDTO.Description,
                IsActive = restaurantRequestDTO.IsActive,
                LogoUrl = restaurantRequestDTO.LogoUrl
            };
            var updated = await _restaurantRepository.UpdateRestaurantAsync(id, restaurant);
            return updated != null ? MapToRestaurantReponseDTO(updated) : null;
        }
        public async Task<bool> DeleteRestaurantAsync(int id, int userId)
        {
            var existingRestaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (existingRestaurant == null || existingRestaurant.UserID != userId)
            return false;
            return await _restaurantRepository.DeleteRestaurantAsync(id);
        }
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
