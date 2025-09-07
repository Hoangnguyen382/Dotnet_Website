namespace Layout_Client.Model.DTO
{
    public class CartItemDTO
    {
        public int MenuItemID { get; set; }
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public int RestaurantID { get; set; }
    }
}
