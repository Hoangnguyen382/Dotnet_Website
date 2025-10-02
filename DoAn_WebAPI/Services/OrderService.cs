using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPromoCodeRepository _promoCodeRepo;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IComboRepository _comboRepository;

        public OrderService(IOrderRepository orderRepo, IOrderDetailRepository orderDetailRepo,
        IMenuItemRepository menuItemRepository, IUserRepository userRepository, IPromoCodeRepository promoCodeRepo, IPromoCodeService promoCodeService, IComboRepository comboRepository)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _menuItemRepository = menuItemRepository;
            _userRepository = userRepository;
            _promoCodeRepo = promoCodeRepo;
            _promoCodeService = promoCodeService;
            _comboRepository = comboRepository;
        }
        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? date = null)
        {
            var orders = await _orderRepo.GetOrdersByRestaurantIdAsync(restaurantId, date);
            return orders.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToResponseDTO);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null) return null;
            return MapToResponseDTO(order);
        }
        public async Task<OrderResponseDTO> CreateOrderAsync(int userId, int restaurantID, OrderRequestDTO dto, List<OrderDetailRequestDTO> orderDetails)
        {
            if (orderDetails == null || !orderDetails.Any())
                throw new ArgumentException("Đơn hàng phải có ít nhất một món ăn.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Người dùng không tồn tại.");

            var order = new Order
            {
                UserID = userId,
                RestaurantID = restaurantID,
                DeliveryAddress = dto.DeliveryAddress,
                PaymentStatus = dto.PaymentStatus,
                Note = dto.Note,
                PhoneNumber = dto.PhoneNumber,
                PaymentMethod = dto.PaymentMethod,
                Status = dto.Status,
                OrderDate = DateTime.UtcNow,
                OrderDetails = new List<OrderDetail>()
            };

            decimal totalAmount = 0m;
            int totalQuantity = 0;

            foreach (var detailDto in orderDetails)
            {
                if (detailDto.Quantity < 1)
                    throw new ArgumentException("Số lượng mỗi sản phẩm/combo phải ít nhất là 1.");

                if (detailDto.ComboID.HasValue) // 👉 Ưu tiên combo trước
                {
                    var combo = await _comboRepository.GetComboByIdAsync(detailDto.ComboID.Value);
                    if (combo == null)
                        throw new ArgumentException($"Combo với ID {detailDto.ComboID} không tồn tại.");

                    decimal comboPrice = combo.Price;
                    totalAmount += comboPrice * detailDto.Quantity;
                    totalQuantity += detailDto.Quantity;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        ComboID = detailDto.ComboID,
                        Quantity = detailDto.Quantity,
                        UnitPrice = comboPrice,
                    });
                }
                else if (detailDto.MenuItemID.HasValue)
                {
                    var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(detailDto.MenuItemID.Value);
                    if (menuItem == null)
                        throw new ArgumentException($"Món ăn với ID {detailDto.MenuItemID} không tồn tại.");

                    decimal unitPrice = menuItem.SellingPrice;
                    totalAmount += unitPrice * detailDto.Quantity;
                    totalQuantity += detailDto.Quantity;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        MenuItemID = detailDto.MenuItemID,
                        Quantity = detailDto.Quantity,
                        UnitPrice = unitPrice,
                    });
                }
                else
                {
                    throw new ArgumentException("OrderDetail phải có MenuItemID hoặc ComboID.");
                }
            }

            // ✅ Áp dụng mã giảm giá nếu có
            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var (discount, promoCodeId) = await _promoCodeService.ValidatePromoCodeAsync(dto.Code, restaurantID, totalAmount, totalQuantity);
                totalAmount -= discount;
                order.PromoCodeID = promoCodeId;
            }
            // Bảo đảm tổng tiền không âm
            if (totalAmount < 0) totalAmount = 0;

            order.TotalAmount = totalAmount;

            await _orderRepo.CreateOrderAsync(order);
            return MapToResponseDTO(order);
        }
        public async Task MarkAsPaidAsync(string orderId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(int.Parse(orderId));
            if (order != null)
            {
                order.PaymentStatus = "Thành Công";
                await _orderRepo.UpdateOrderAsync(order);
            }
        }
        public async Task MarkAsFailedAsync(string orderId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(int.Parse(orderId));
            if (order != null)
            {
                order.PaymentStatus = "Thất Bại";
                await _orderRepo.UpdateOrderAsync(order);
            }
        }
        public async Task<bool> UpdateOrderAsync(int id, OrderRequestDTO dto)
        {
            var existOrder = await _orderRepo.GetOrderByIdAsync(id);
            if (existOrder == null) return false;
            existOrder.Status = dto.Status;
            await _orderRepo.UpdateOrderAsync(existOrder);
            return true;
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepo.DeleteOrderAsync(id);
        }

        private OrderResponseDTO MapToResponseDTO(Order order)
        {
            return new OrderResponseDTO
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                OrderDate = order.OrderDate,
                TotalAmount = (decimal)order.TotalAmount,
                Status = order.Status,
                DeliveryAddress = order.DeliveryAddress,
                PaymentStatus = order.PaymentStatus,
                Note = order.Note,
                PhoneNumber = order.PhoneNumber,
                PaymentMethod = order.PaymentMethod
            };
        }
    }

}