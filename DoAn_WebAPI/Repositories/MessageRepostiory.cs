using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(int conversationId, int skip = 0, int take = 50)
                => await _context.Messages
                    .Where(m => m.ConversationId == conversationId)
                    .Include(m => m.Images)
                    .OrderBy(m => m.SentAt)
                    .Skip(skip).Take(take)
                    .ToListAsync();
        }
    }