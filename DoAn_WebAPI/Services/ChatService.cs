using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
namespace DoAn_WebAPI.Services
{
    // ChatService.cs
public class ChatService : IChatService
{
    private readonly IConversationRepository _convRepo;
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _msgRepo;
    private readonly IMessageImageRepository _imgRepo;

    public ChatService(IConversationRepository convRepo, IUserRepository userRepository, IMessageRepository msgRepo, IMessageImageRepository imgRepo)
    {
        _convRepo = convRepo;
        _userRepository = userRepository;
        _msgRepo = msgRepo;
        _imgRepo = imgRepo;
    }

    public async Task<ConversationResponseDTO> CreateConversationAsync(CreateConversationRequestDTO dto)
    {
        var existing = await _convRepo.GetByOrderIdAsync(dto.OrderId);
        if (existing != null)
        {
            return new ConversationResponseDTO
            {
                ConversationId = existing.ConversationId,
                OrderId = existing.OrderId,
                CustomerId = existing.CustomerId,
                RestaurantId = existing.RestaurantId,
                CreatedAt = existing.CreatedAt
            };
        }

        var conv = new Conversation
        {
            OrderId = dto.OrderId,
            CustomerId = dto.CustomerId,
            RestaurantId = dto.RestaurantId,
            CreatedAt = DateTime.Now
        };

        var created = await _convRepo.CreateAsync(conv);
        return new ConversationResponseDTO
        {
            ConversationId = created.ConversationId,
            OrderId = created.OrderId,
            CustomerId = created.CustomerId,
            RestaurantId = created.RestaurantId,
            CreatedAt = created.CreatedAt
        };
    }
    public async Task<ConversationResponseDTO?> GetConversationByOrderAsync(int orderId)
    {
        var conv = await _convRepo.GetByOrderIdAsync(orderId);
        if (conv == null) return null;
        return new ConversationResponseDTO
        {
            ConversationId = conv.ConversationId,
            OrderId = conv.OrderId,
            CustomerId = conv.CustomerId,
            RestaurantId = conv.RestaurantId,
            CreatedAt = conv.CreatedAt
        };
    }
    public async Task<IEnumerable<ConversationResponseDTO>> GetConversationsByCustomerAsync(int customerId)
    {
        var list = await _convRepo.GetByCustomerIdAsync(customerId);
        return list.Select(c => new ConversationResponseDTO {
            ConversationId = c.ConversationId,
            OrderId = c.OrderId,
            CustomerId = c.CustomerId,
            RestaurantId = c.RestaurantId,
            CreatedAt = c.CreatedAt
        });
    }

    public async Task<IEnumerable<ConversationResponseDTO>> GetConversationsByRestaurantAsync(int restaurantId)
    {
        var list = await _convRepo.GetByRestaurantIdAsync(restaurantId);
        return list.Select(c => new ConversationResponseDTO {
            ConversationId = c.ConversationId,
            OrderId = c.OrderId,
            CustomerId = c.CustomerId,
            RestaurantId = c.RestaurantId,
            CreatedAt = c.CreatedAt
        });
    }

    public async Task<MessageResponseDTO> SendMessageAsync(int userId, CreateMessageRequestDTO dto)
    {
        var conv = await _convRepo.GetByIdAsync(dto.ConversationId);
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (conv == null) throw new KeyNotFoundException("Conversation not found.");

        var message = new Message
        {
            ConversationId = dto.ConversationId,
            SenderId = userId,
            SenderRole = user.Role,
            Content = dto.Content ?? "",
            SentAt = DateTime.Now,
            Images = new List<MessageImage>()
        };

        var created = await _msgRepo.CreateAsync(message);

        if (dto.ImageUrls != null && dto.ImageUrls.Any())
        {
            foreach (var url in dto.ImageUrls)
            {
                var img = new MessageImage
                {
                    MessageId = created.MessageId,
                    ImageUrl = url
                };
                await _imgRepo.CreateAsync(img);
            }
            created.Images = await _imgRepo.GetByMessageIdAsync(created.MessageId);
        }
        return new MessageResponseDTO
        {
            MessageId = created.MessageId,
            ConversationId = created.ConversationId,
            SenderId = created.SenderId,
            SenderRole = created.SenderRole,
            Content = created.Content,
            SentAt = created.SentAt,
            Images = created.Images?.Select(i => new MessageImageDTO { MessageImageId = i.MessageImageId, ImageUrl = i.ImageUrl }).ToList() ?? new()
        };
    }

    public async Task<IEnumerable<MessageResponseDTO>> GetMessagesAsync(int conversationId, int skip = 0, int take = 100)
    {
        var messages = await _msgRepo.GetMessagesByConversationIdAsync(conversationId, skip, take);
        return messages.Select(m => new MessageResponseDTO {
            MessageId = m.MessageId,
            ConversationId = m.ConversationId,
            SenderId = m.SenderId,
            SenderRole = m.SenderRole,
            Content = m.Content,
            SentAt = m.SentAt,
            Images = m.Images?.Select(i => new MessageImageDTO { MessageImageId = i.MessageImageId, ImageUrl = i.ImageUrl }).ToList() ?? new()
        });
    }
}

}
