namespace Layout_Admin.Model.DTO
{
    public class ComboDetailRequestDTO
    {
        public int ComboID { get; set; }
        public int MenuItemID { get; set; }
        public int Quantity { get; set; } = 1;
    }
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