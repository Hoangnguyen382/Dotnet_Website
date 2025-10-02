using DoAn_WebAPI.Interfaces.IRepository;
using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;

namespace DoAn_WebAPI.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IMenuItemRepository _menuItemRepository;
        public OrderDetailService(IOrderDetailRepository orderDetailRepo, IMenuItemRepository menuItemRepository)
        {
            _orderDetailRepo = orderDetailRepo;
            _menuItemRepository = menuItemRepository;
        }
        public async Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var details = await _orderDetailRepo.GetOrderDetailsByOrderIdAsync(orderId);
            return details.Select(od => new OrderDetailResponseDTO
            {
                OrderDetailID = od.OrderDetailID,
                OrderID = od.OrderID,
                MenuItemID = od.MenuItemID ?? 0,
                ComboID = od.ComboID ?? 0,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            });
        }
        public async Task<OrderDetailResponseDTO> GetOrderDetailByIdAsync(int id)
        {
            var detail = await _orderDetailRepo.GetOrderDetailByIdAsync(id);
            if (detail == null) return null;
            return new OrderDetailResponseDTO
            {
                OrderDetailID = detail.OrderDetailID,
                OrderID = detail.OrderID,
                MenuItemID = detail.MenuItemID ?? 0,
                ComboID = detail.ComboID ?? 0,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };
        }
        public async Task<OrderDetailResponseDTO> CreateOrderDetailAsync(OrderDetailRequestDTO dto)
        {
            var detail = new OrderDetail
            {
                OrderID = dto.OrderID,
                MenuItemID = dto.MenuItemID,
                ComboID = dto.ComboID,
                Quantity = dto.Quantity,
                UnitPrice = await GetUnitPriceByMenuItemIdAsync(dto.MenuItemID ?? 0)
            };
            await _orderDetailRepo.CreateOrderDetailAsync(detail);
            return new OrderDetailResponseDTO
            {
                OrderDetailID = detail.OrderDetailID,
                OrderID = detail.OrderID,
                MenuItemID = detail.MenuItemID ?? 0,
                ComboID = detail.ComboID ?? 0,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };
        }
        public async Task<bool> UpdateOrderDetailAsync(int id, OrderDetailRequestDTO dto)
        {
            var existDetail = await _orderDetailRepo.GetOrderDetailByIdAsync(id);
            if (existDetail == null) return false;
            existDetail.Quantity = dto.Quantity;
            existDetail.UnitPrice = await GetUnitPriceByMenuItemIdAsync(dto.MenuItemID ?? 0);
            await _orderDetailRepo.UpdateOrderDetailAsync(existDetail);
            return true;
        }
        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            return await _orderDetailRepo.DeleteOrderDetailAsync(id);
        }
        private async Task<decimal> GetUnitPriceByMenuItemIdAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new ArgumentException($"Món ăn với ID {menuItemId} không tồn tại.");
            }
            return menuItem.Price;
        }
    }    
}