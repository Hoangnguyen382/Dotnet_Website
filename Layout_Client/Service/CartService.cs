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

        if (cart.Count > 0 && cart.Any(i => i.RestaurantID != item.RestaurantID))
        {
            return false;
        }

        CartItemDTO? existing = null;

        if (item.MenuItemID != null)
            existing = cart.FirstOrDefault(i => i.MenuItemID == item.MenuItemID);
        else if (item.ComboID != null)
            existing = cart.FirstOrDefault(i => i.ComboID == item.ComboID);

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

    public async Task RemoveFromCartAsync(int? menuItemId, int? comboId)
    {
        var cart = await GetCartAsync();
        if (menuItemId != null)
            cart.RemoveAll(x => x.MenuItemID == menuItemId);
        else if (comboId != null)
            cart.RemoveAll(x => x.ComboID == comboId);

        await _localStorage.SetItemAsync(CartKey, cart);
        NotifyCartChanged();
    }


    public async Task ClearCartAsync()
    {
        await _localStorage.RemoveItemAsync(CartKey);
        NotifyCartChanged();
    }
}
