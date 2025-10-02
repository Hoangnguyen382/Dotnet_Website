namespace Layout_Client.Model.DTO
{
    public class ComboDetailResponseDTO
    {
        public int ComboDetailID { get; set; }
        public int ComboID { get; set; }
        public int MenuItemID { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public string MenuItemImage { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
}