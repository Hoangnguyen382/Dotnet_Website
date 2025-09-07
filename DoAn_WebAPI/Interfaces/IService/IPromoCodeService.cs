using DoAn_WebAPI.Models.DTOs;
using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IService
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCodeResponseDTO>> GetAllPromoCodesAsync(int restaurantId, int userId);
        Task<IEnumerable<PromoCodeResponseDTO>> GetPromoCodesByRestaurantAsync(int restaurantId);

        Task<PromoCodeResponseDTO> GetPromoCodeByIdAsync(int id);
        Task<PromoCodeResponseDTO> CreatePromoCodeAsync(int userId, int restaurantId, PromoCodeRequestDTO dto);
        Task<PromoCodeResponseDTO> UpdatePromoCodeAsync(int id, int userId, PromoCodeRequestDTO dto);
        Task<bool> DeletePromoCodeAsync(int id, int userId);
        Task<PromoCode> GetValidPromoCodeByCodeAsync(string code, int restaurantId, int userId);
        Task<(decimal discount, int? promoCodeId)> ValidatePromoCodeAsync(
        string code, int restaurantId, decimal totalAmount, int totalQuantity);
    }
}
