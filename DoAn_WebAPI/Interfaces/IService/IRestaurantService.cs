using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantResponseDTO>> GetAllRestaurantAsync(string? search, int page, int pageSize);
        Task<IEnumerable<RestaurantResponseDTO>> GetAllRestaurantByUserAsync(int userId);
        Task<RestaurantResponseDTO> GetRestaurantByIdAsync(int id);
        Task<RestaurantResponseDTO> CreateRestaurantAsync(RestaurantRequestDTO restaurantRequestDTO, int userId);
        Task<RestaurantResponseDTO> UpdateRestaurantAsync(int id,int userId, RestaurantRequestDTO restaurantRequestDTO);
        Task<bool> DeleteRestaurantAsync(int id, int userId);
    }
}