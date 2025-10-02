using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repositories
{
    public class ComboRepository : IComboRepository
    {
        private readonly ApplicationDbContext _context;

        public ComboRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #region Admin
        public async Task<IEnumerable<Combo>> GetCombosByRestaurantAsync(int restaurantId, int page, int pageSize)
        {
            return await _context.Combos
           .Where(c => c.RestaurantID == restaurantId)
           .Include(c => c.ComboDetails)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
        public async Task<Combo?> GetComboByIdAsync(int comboId)
        {
            return await _context.Combos
            .Include(c => c.ComboDetails)
            .FirstOrDefaultAsync(c => c.ComboID == comboId);
        }

        public async Task<Combo> CreateComboAsync(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return combo;
        }

        public async Task<Combo?> UpdateComboAsync(Combo combo)
        {
            _context.Combos.Update(combo);
            await _context.SaveChangesAsync();
            return combo;
        }

        public async Task<bool> DeleteComboAsync(int comboId)
        {
            var combo = await _context.Combos.FindAsync(comboId);
            if (combo == null)
            {
                return false;
            }
            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Client
        public async Task<IEnumerable<Combo>> GetAvailableCombosByRestaurantAsync(int restaurantId, int page, int pageSize)
        {
            return await _context.Combos
           .Where(c => c.RestaurantID == restaurantId)
           .Include(c => c.ComboDetails)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }

        public async Task<Combo?> GetAvailableComboByIdAsync(int comboId)
        {
            return await _context.Combos
            .Include(c => c.ComboDetails)
            .FirstOrDefaultAsync(c => c.ComboID == comboId);
        }
        #endregion
    }
}