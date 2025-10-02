using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class ComboDetailService : IComboDetailService
    {
        private readonly IComboDetailRepository _comboDetailRepository;
        private readonly IComboRepository _comboRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUserRepository _userRepository;

        public ComboDetailService(IComboDetailRepository comboDetailRepository,IComboRepository comboRepository,IMenuItemRepository menuItemRepository,IUserRepository userRepository)
        {
            _comboDetailRepository = comboDetailRepository;
            _comboRepository = comboRepository;
            _menuItemRepository = menuItemRepository;
            _userRepository = userRepository;
        }
        #region Admin
        public async Task<IEnumerable<ComboDetailResponseDTO>> GetDetailsByComboIdAsync(int comboId, int userId)
        {
            var combo = await _comboRepository.GetComboByIdAsync(comboId);
            if (combo == null) return Enumerable.Empty<ComboDetailResponseDTO>();

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xem chi tiết combo này.");

            var details = await _comboDetailRepository.GetComboDetailsByComboIdAsync(comboId);
            return details.Select(MapToResponseDTO);
        }

        public async Task<ComboDetailResponseDTO?> GetDetailByIdAsync(int comboDetailId, int userId)
        {
            var detail = await _comboDetailRepository.GetComboDetailByIdAsync(comboDetailId);
            if (detail == null) return null;

            var combo = await _comboRepository.GetComboByIdAsync(detail.ComboID);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (combo == null || user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xem chi tiết này.");

            return MapToResponseDTO(detail);
        }

        public async Task<ComboDetailResponseDTO> AddDetailAsync(ComboDetailRequestDTO dto, int userId)
        {
            var combo = await _comboRepository.GetComboByIdAsync(dto.ComboID);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (combo == null || user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Không thể thêm chi tiết cho combo nhà hàng khác.");

            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(dto.MenuItemID);
            if (menuItem == null || menuItem.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Món ăn không thuộc nhà hàng này.");

            // ✅ Kiểm tra xem món ăn đã có trong combo chưa
            var existingDetail = (await _comboDetailRepository.GetComboDetailsByComboIdAsync(dto.ComboID))
                                    .FirstOrDefault(d => d.MenuItemID == dto.MenuItemID);

            if (existingDetail != null)
            {
                // Nếu đã có thì cộng thêm số lượng
                existingDetail.Quantity += dto.Quantity;
                var updated = await _comboDetailRepository.UpdateComboDetailAsync(existingDetail);
                return MapToResponseDTO(updated);
            }
            else
            {
                // Nếu chưa có thì thêm mới
                var detail = new ComboDetail
                {
                    ComboID = dto.ComboID,
                    MenuItemID = dto.MenuItemID,
                    Quantity = dto.Quantity
                };

                var created = await _comboDetailRepository.AddComboDetailAsync(detail);
                return MapToResponseDTO(created);
            }
        }

        public async Task<ComboDetailResponseDTO?> UpdateDetailAsync(int comboDetailId, ComboDetailRequestDTO dto, int userId)
        {
            var existing = await _comboDetailRepository.GetComboDetailByIdAsync(comboDetailId);
            if (existing == null) return null;

            var combo = await _comboRepository.GetComboByIdAsync(existing.ComboID);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (combo == null || user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền sửa chi tiết này.");

            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(dto.MenuItemID);
            if (menuItem == null || menuItem.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Món ăn không thuộc nhà hàng này.");

            existing.MenuItemID = dto.MenuItemID;
            existing.Quantity = dto.Quantity;

            var updated = await _comboDetailRepository.UpdateComboDetailAsync(existing);
            return updated != null ? MapToResponseDTO(updated) : null;
        }

        public async Task<bool> DeleteDetailAsync(int comboDetailId, int userId)
        {
            var existing = await _comboDetailRepository.GetComboDetailByIdAsync(comboDetailId);
            if (existing == null) return false;

            var combo = await _comboRepository.GetComboByIdAsync(existing.ComboID);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (combo == null || user == null || user.RestaurantID != combo.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa chi tiết này.");

            return await _comboDetailRepository.DeleteComboDetailAsync(comboDetailId);
        }

        #endregion

        #region Client
        public async Task<IEnumerable<ComboDetailResponseDTO>> GetAvailableDetailsByComboIdAsync(int comboId)
        {
            var details = await _comboDetailRepository.GetComboDetailsByComboIdAsync(comboId);
            return details.Select(MapToResponseDTO);
        }
        public async Task<ComboDetailResponseDTO?> GetAvailableDetailByIdAsync(int comboDetailId)
        {
            var detail = await _comboDetailRepository.GetComboDetailByIdAsync(comboDetailId);
            if (detail == null) return null;
            return MapToResponseDTO(detail);
        }
        
        #endregion
        private ComboDetailResponseDTO MapToResponseDTO(ComboDetail detail)
        {
            return new ComboDetailResponseDTO
            {
                ComboDetailID = detail.ComboDetailID,
                ComboID = detail.ComboID,
                MenuItemID = detail.MenuItemID,
                MenuItemName = detail.MenuItem?.Name ?? string.Empty,
                MenuItemImage = detail.MenuItem?.ImageUrl ?? string.Empty,
                Quantity = detail.Quantity
            };
        }
    }
}
