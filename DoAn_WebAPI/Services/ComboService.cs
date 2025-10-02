using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;
        private readonly IUserRepository _userRepository;
        public ComboService(IComboRepository comboRepository, IUserRepository userRepository)
        {
            _comboRepository = comboRepository;
            _userRepository = userRepository;
        }
        #region Admin
        public async Task<IEnumerable<ComboResponseDTO>> GetCombosByRestaurantAsync(int restaurantId, int userId, int page, int pageSize)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantId)
                throw new UnauthorizedAccessException("Bạn không có quyền xem combo của nhà hàng này.");

            var combos = await _comboRepository.GetCombosByRestaurantAsync(restaurantId, page, pageSize);
            return combos.Select(MapToComboResponseDTO);
        }
        public async Task<ComboResponseDTO?> GetComboByIdAsync(int comboId, int userId)
        {
            var combo = await _comboRepository.GetComboByIdAsync(comboId);
            if (combo == null) return null;
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xem combo này.");
            return MapToComboResponseDTO(combo);
        }
        public async Task<ComboResponseDTO> CreateComboAsync(ComboRequestDTO dto, int userId, int restaurantId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantId)
                throw new UnauthorizedAccessException("Không thể tạo combo cho nhà hàng khác.");
            var combo = new Combo
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive,
                RestaurantID = restaurantId,
            };
            var created = await _comboRepository.CreateComboAsync(combo);
            return MapToComboResponseDTO(created);
        }
        public async Task<ComboResponseDTO?> UpdateComboAsync(int comboId, ComboRequestDTO dto, int userId)
        {
            var combo = await _comboRepository.GetComboByIdAsync(comboId);
            if (combo == null) return null;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền sửa combo này.");

            combo.Name = dto.Name;
            combo.Description = dto.Description;
            combo.Price = dto.Price;
            combo.ImageUrl = dto.ImageUrl;
            combo.IsActive = dto.IsActive;
            var updated = await _comboRepository.UpdateComboAsync(combo);
            return updated != null ? MapToComboResponseDTO(updated) : null;
        }
        public async Task<bool> DeleteComboAsync(int comboId, int userId)
        {
            var combo = await _comboRepository.GetComboByIdAsync(comboId);
            if (combo == null) return false;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa combo này.");
            return await _comboRepository.DeleteComboAsync(comboId);
        }
        #endregion
        
        #region Client
        public async Task<IEnumerable<ComboResponseDTO>> GetAvailableCombosByRestaurantAsync(int restaurantId, int page, int pageSize)
        {
            var combos = await _comboRepository.GetAvailableCombosByRestaurantAsync(restaurantId, page, pageSize);
            return combos.Select(MapToComboResponseDTO);
        }
        public async Task<ComboResponseDTO?> GetAvailableComboByIdAsync(int comboId)
        {
            var combo = await _comboRepository.GetAvailableComboByIdAsync(comboId);
            if (combo == null) return null;
            return MapToComboResponseDTO(combo);
        }
        #endregion
        private ComboResponseDTO MapToComboResponseDTO(Combo combo)
        {
            return new ComboResponseDTO
            {
                ComboID = combo.ComboID,
                RestaurantID = combo.RestaurantID,
                Name = combo.Name,
                Description = combo.Description,
                Price = combo.Price,
                ImageUrl = combo.ImageUrl,
                IsActive = combo.IsActive,
            };
        }
    }
}