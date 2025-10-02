using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repositories
{
    public class ComboDetailRepository : IComboDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public ComboDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #region Admin
        public async Task<IEnumerable<ComboDetail>> GetComboDetailsByComboIdAsync(int comboId)
        {
            return await _context.ComboDetails
                .Include(cd => cd.MenuItem)
                .Where(cd => cd.ComboID == comboId)
                .ToListAsync();
        }
        public async Task<ComboDetail?> GetComboDetailByIdAsync(int id)
        {
            return await _context.ComboDetails
                .Include(cd => cd.MenuItem)
                .FirstOrDefaultAsync(cd => cd.ComboDetailID == id);
        }

        public async Task<ComboDetail> AddComboDetailAsync(ComboDetail detail)
        {
            _context.ComboDetails.Add(detail);
            await _context.SaveChangesAsync();
            return detail;
        }

        public async Task<ComboDetail?> UpdateComboDetailAsync(ComboDetail detail)
        {
            var existing = await _context.ComboDetails.FindAsync(detail.ComboDetailID);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(detail);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteComboDetailAsync(int id)
        {
            var detail = await _context.ComboDetails.FindAsync(id);
            if (detail == null) return false;

            _context.ComboDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Client
        public async Task<IEnumerable<ComboDetail>> GetAvailableComboDetailsByComboIdAsync(int comboId)
        {
            return await _context.ComboDetails
                .Include(cd => cd.MenuItem)
                .Where(cd => cd.ComboID == comboId && cd.MenuItem.IsAvailable)
                .ToListAsync();
        }
        public async Task<ComboDetail?> GetAvailableComboDetailByIdAsync(int id)
        {
            return await _context.ComboDetails
                .Include(cd => cd.MenuItem)
                .FirstOrDefaultAsync(cd => cd.ComboDetailID == id && cd.MenuItem.IsAvailable);
        }
        #endregion
    }
}
