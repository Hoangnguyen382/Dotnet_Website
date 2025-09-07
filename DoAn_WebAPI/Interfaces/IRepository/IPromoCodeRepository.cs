using DoAn_WebAPI.Models;

namespace DoAn_WebAPI.Interfaces.IRepository
{
    public interface IPromoCodeRepository
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync(int restaurantId);
        Task<PromoCode> GetPromoCodeByIdAsync(int id);
        Task<PromoCode> GetPromoCodeByCodeAsync(string code);
        Task<PromoCode> CreatePromoCodeAsync(PromoCode promoCode);
        Task<PromoCode> UpdatePromoCodeAsync(int id, PromoCode promoCode);
        Task<bool> DeletePromoCodeAsync(int id);
    }
}
