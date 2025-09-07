using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn_WebAPI.Repositories
{
    public class PromoCodeRepository : IPromoCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public PromoCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync(int restaurantId)
        {
            return await _context.PromoCodes
                         .Where(p => p.RestaurantID == restaurantId)
                         .ToListAsync();
        }

        public async Task<PromoCode?> GetPromoCodeByIdAsync(int id)
        {
            return await _context.PromoCodes.FindAsync(id);
        }

        public async Task<PromoCode?> GetPromoCodeByCodeAsync(string code)
        {
            return await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<PromoCode> CreatePromoCodeAsync(PromoCode promoCode)
        {
            _context.PromoCodes.Add(promoCode);
            await _context.SaveChangesAsync();
            return promoCode;
        }

        public async Task<PromoCode> UpdatePromoCodeAsync(int id,PromoCode promoCode)
        {
            var existingpromo= await _context.PromoCodes.FindAsync(id);
            // nếu không tồn tại thì trả về null
            if (existingpromo == null)
            {
                return null;
            }
            // nếu có tồn tại thì cập nhật thông tin
            existingpromo.Code = promoCode.Code;
            existingpromo.Description = promoCode.Description;
            existingpromo.Type = promoCode.Type;
            existingpromo.StartDate = promoCode.StartDate;
            existingpromo.ExpiryDate = promoCode.ExpiryDate;
            existingpromo.IsActive = promoCode.IsActive;
            existingpromo.RestaurantID = promoCode.RestaurantID;
            existingpromo.MinOrderAmount = promoCode.MinOrderAmount;
            existingpromo.MinQuantity = promoCode.MinQuantity;
            existingpromo.GiftMenuItemID = promoCode.GiftMenuItemID;
            existingpromo.DiscountAmount = promoCode.DiscountAmount;
            existingpromo.DiscountPercent = promoCode.DiscountPercent;

            _context.PromoCodes.Update(existingpromo);
            await _context.SaveChangesAsync();
            return promoCode;
        }

        public async Task<bool> DeletePromoCodeAsync(int id)
        {
            var promoCode = await _context.PromoCodes.FindAsync(id);
            if (promoCode == null) return false;
            _context.PromoCodes.Remove(promoCode);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
