using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace DoAn_WebAPI.Repositories
{
    
    public class MessageImageRepository : IMessageImageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageImage> CreateAsync(MessageImage image)
        {
            _context.MessageImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<MessageImage>> GetByMessageIdAsync(int messageId)
            => await _context.MessageImages.Where(i => i.MessageId == messageId).ToListAsync();
    }
}