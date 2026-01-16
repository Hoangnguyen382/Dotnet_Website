using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn_WebAPI.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepo;
        private readonly IRestaurantRepository _resRepo;
        private readonly IUserRepository _userRepository;

        public PromoCodeService(IPromoCodeRepository promoCodeRepo, IRestaurantRepository resRepo, IUserRepository userRepository)
        {
            _promoCodeRepo = promoCodeRepo;
            _resRepo = resRepo;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<PromoCodeResponseDTO>> GetAllPromoCodesAsync(int restaurantId, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != restaurantId)
                throw new UnauthorizedAccessException("Bạn không có quyền xem mã giảm giá của nhà hàng này.");

            var promoCodes = await _promoCodeRepo.GetAllPromoCodesAsync(restaurantId);
            return promoCodes.Select(p => MapToResponseDTO(p));
        }
        // Lấy danh sách promo code pcho client
        public async Task<IEnumerable<PromoCodeResponseDTO>> GetPromoCodesByRestaurantAsync(int restaurantId)
        {
            var promoCodes = await _promoCodeRepo.GetAllPromoCodesAsync(restaurantId);

            // Lọc các mã còn hiệu lực và đang hoạt động
            var now = DateTime.UtcNow;
            var validPromoCodes = promoCodes
                .Where(p => p.IsActive &&
                            (!p.StartDate.HasValue || p.StartDate.Value <= now) &&
                            (!p.ExpiryDate.HasValue || p.ExpiryDate.Value >= now))
                .ToList();

            return validPromoCodes.Select(MapToResponseDTO);
        }

        public async Task<PromoCodeResponseDTO> GetPromoCodeByIdAsync(int id)
        {
            var promoCode = await _promoCodeRepo.GetPromoCodeByIdAsync(id);
            if (promoCode == null)
            { 
                return null;
            }
            return MapToResponseDTO(promoCode);
        }
        public async Task<PromoCodeResponseDTO> CreatePromoCodeAsync(int userId, int restaurantId, PromoCodeRequestDTO dto)
        {
            var restaurant = await _resRepo.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null || restaurant.UserID != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to add items to this restaurant.");
            }

            var resList = await _resRepo.GetRestaurantsByUserIdAsync(userId);
            var res = resList.FirstOrDefault();
            var existing = await _promoCodeRepo.GetPromoCodeByCodeAsync(dto.Code);
            if (existing != null)
                throw new ArgumentException($"Mã {dto.Code} đã tồn tại.");

            var promoCode = new PromoCode
            {
                Code = dto.Code,
                Description = dto.Description,
                Type = dto.Type,
                StartDate = dto.StartDate,
                ExpiryDate = dto.ExpiryDate,
                IsActive = dto.IsActive,
                RestaurantID = restaurantId,
                MinOrderAmount = dto.MinOrderAmount,
                MinQuantity = dto.MinQuantity,
                GiftMenuItemID = dto.GiftMenuItemID,
                DiscountAmount = dto.DiscountAmount,
                DiscountPercent = dto.DiscountPercent
            };
            var created = await _promoCodeRepo.CreatePromoCodeAsync(promoCode);
            return MapToResponseDTO(created);
        }

        public async Task<PromoCodeResponseDTO> UpdatePromoCodeAsync(int id, int userId, PromoCodeRequestDTO dto)
        {
            var promoCode = await _promoCodeRepo.GetPromoCodeByIdAsync(id);
            if (promoCode == null)
                return null;    
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != promoCode.RestaurantID)
                throw new UnauthorizedAccessException("Bạn không có quyền tạo mã giảm giá cho nhà hàng này.");

            promoCode.Code = dto.Code;
            promoCode.Description = dto.Description;
            promoCode.Type = dto.Type;
            promoCode.StartDate = dto.StartDate;
            promoCode.ExpiryDate = dto.ExpiryDate;
            promoCode.IsActive = dto.IsActive;
            promoCode.RestaurantID = promoCode.RestaurantID;
            promoCode.MinOrderAmount = dto.MinOrderAmount;
            promoCode.MinQuantity = dto.MinQuantity;
            promoCode.GiftMenuItemID = dto.GiftMenuItemID;
            promoCode.DiscountAmount = dto.DiscountAmount;
            promoCode.DiscountPercent = dto.DiscountPercent;
            var updated = await _promoCodeRepo.UpdatePromoCodeAsync(id, promoCode);
            return MapToResponseDTO(updated);
        }

        public async Task<bool> DeletePromoCodeAsync(int id, int userId)
        {
            var existing = await _promoCodeRepo.GetPromoCodeByIdAsync(id);
            if (existing == null)
                return false;
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.RestaurantID != existing.RestaurantID)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa mã giảm giá này.");
            }

            return await _promoCodeRepo.DeletePromoCodeAsync(id);
        }
        
        public async Task<PromoCode> GetValidPromoCodeByCodeAsync(string code, int restaurantId, int userId)
        {
            var promoCode = await _promoCodeRepo.GetPromoCodeByCodeAsync(code);
            if (promoCode == null || !promoCode.IsActive)
                return null;

            if (promoCode.RestaurantID != restaurantId)
                return null;

            var now = DateTime.UtcNow;

            if (promoCode.StartDate.HasValue && promoCode.StartDate.Value > now)
                return null;

            if (promoCode.ExpiryDate.HasValue && promoCode.ExpiryDate.Value < now)
                return null;

            return promoCode;
        }
        public async Task<ValidatePromoDTO> ValidatePromoCodeAsync(string code, int restaurantId, decimal totalAmount, int totalQuantity)
        {
            var result = new ValidatePromoDTO();
            if (string.IsNullOrWhiteSpace(code))
            {
                result.Error = "Mã giảm giá không hợp lệ.";
                return result;
            }
            var promo = await _promoCodeRepo.GetPromoCodeByCodeAsync(code);
            if (promo == null || !promo.IsActive)
            {
                result.Error = "Mã giảm giá không hợp lệ.";
                return result;
            }
            if (promo.RestaurantID != restaurantId)
            {
                result.Error = "Mã giảm giá không áp dụng cho nhà hàng này.";
                return result;
            }
            var now = DateTime.UtcNow;
            if ((promo.StartDate.HasValue && promo.StartDate > now) ||
                (promo.ExpiryDate.HasValue && promo.ExpiryDate < now))
            {
                result.Error = "Mã giảm giá đã hết hạn hoặc chưa bắt đầu.";
                return result;
            }

            decimal discount = 0;

            if (promo.Type == PromoCodeType.AmountDiscount)
            {
                if (!promo.MinOrderAmount.HasValue || totalAmount < promo.MinOrderAmount.Value)
                {
                    result.Error = $"Đơn hàng phải tối thiểu {promo.MinOrderAmount:N0} VND.";
                    return result;
                }

                if (promo.DiscountAmount is null or <= 0)
                {
                    result.Error = "Mã giảm giá không có giá trị giảm hợp lệ.";
                    return result;
                }

                discount += promo.DiscountAmount.Value;
            }
            else if (promo.Type == PromoCodeType.QuantityDiscount)
            {
                if (!promo.MinQuantity.HasValue || totalQuantity < promo.MinQuantity.Value)
                {
                    result.Error = $"Bạn phải đặt ít nhất {promo.MinQuantity} món.";
                    return result;
                }

                if (promo.DiscountPercent is null or <= 0)
                {
                    result.Error = "Mã giảm giá không có giá trị giảm hợp lệ.";
                    return result;
                }

               discount = totalAmount * (promo.DiscountPercent.Value / 100);
            }
            result.Discount = discount;
            result.PromoCodeId = promo.PromoCodeID;
            result.Error = null;
            return result;
        }

        private PromoCodeResponseDTO MapToResponseDTO(PromoCode p)
        {
            return new PromoCodeResponseDTO
            {
                PromoCodeID = p.PromoCodeID,
                Code = p.Code,
                Description = p.Description,
                Type = p.Type,
                StartDate = p.StartDate,
                ExpiryDate = p.ExpiryDate,
                IsActive = p.IsActive,
                RestaurantID = p.RestaurantID,
                MinOrderAmount = p.MinOrderAmount,
                MinQuantity = p.MinQuantity,
                GiftMenuItemID = p.GiftMenuItemID,
                DiscountAmount = p.DiscountAmount,
                DiscountPercent = p.DiscountPercent
            };
        }


    }
}
