using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<IEnumerable<Restaurant>> GetRestaurantsByUserIdAsync(int userId);
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(int id, Restaurant restaurant);
        Task<bool>DeleteRestaurantAsync(int id);
    }
}