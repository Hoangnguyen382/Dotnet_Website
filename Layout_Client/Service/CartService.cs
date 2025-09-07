using Blazored.LocalStorage;
using Layout_Client.Model.DTO;

public class CartService
{
    public event Action? CartChanged;
    
    private const string CartKey = "cart_items";
    private readonly ILocalStorageService _localStorage;

    public CartService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    private void NotifyCartChanged()
    {
        CartChanged?.Invoke();
    }
    public async Task<List<CartItemDTO>> GetCartAsync()
        => await _localStorage.GetItemAsync<List<CartItemDTO>>(CartKey) ?? new();

    public async Task<bool> AddToCartAsync(CartItemDTO item)
    {
        var cart = await GetCartAsync();

        // Nếu giỏ hàng không rỗng, kiểm tra RestaurantID
        if (cart.Count > 0 && cart.Any(i => i.RestaurantID != item.RestaurantID))
        {
            // Không cho phép thêm món từ nhà hàng khác
            return false;
        }

        var existing = cart.FirstOrDefault(i => i.MenuItemID == item.MenuItemID);
        if (existing != null)
            existing.Quantity += item.Quantity;
        else
            cart.Add(item);

        await _localStorage.SetItemAsync(CartKey, cart);
        NotifyCartChanged();
        return true;
        
    }
    public async Task SaveCartAsync(List<CartItemDTO> cart)
    {
        await _localStorage.SetItemAsync(CartKey, cart);
    }

    public async Task RemoveFromCartAsync(int menuItemId)
    {
        var cart = await GetCartAsync();
        cart.RemoveAll(x => x.MenuItemID == menuItemId);
        await _localStorage.SetItemAsync(CartKey, cart);
        NotifyCartChanged();
    }

    public async Task ClearCartAsync()
    {
        await _localStorage.RemoveItemAsync(CartKey);
        NotifyCartChanged();
    }
}
