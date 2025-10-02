using DoAn_WebAPI.Data;
using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_WebAPI.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;
        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> GetByOrderIdAsync(int orderId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .ThenInclude(m => m.Images)
                .FirstOrDefaultAsync(c => c.OrderId == orderId);
        }
        public async Task<Conversation?> GetByIdAsync(int id)
        => await _context.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.ConversationId == id);
         public async Task<IEnumerable<Conversation>> GetByCustomerIdAsync(int customerId)
        => await _context.Conversations.Where(c => c.CustomerId == customerId).ToListAsync();

    public async Task<IEnumerable<Conversation>> GetByRestaurantIdAsync(int restaurantId)
        => await _context.Conversations.Where(c => c.RestaurantId == restaurantId).ToListAsync();
        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }
    }
}