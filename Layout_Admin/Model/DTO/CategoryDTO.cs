namespace Layout_Admin.Models.DTO
{
    // DTO cho response (trả về cho client)
    public class CategoryResponseDTO
    {
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int RestaurantID { get; set;}
    }

    // DTO cho request (khi tạo hoặc cập nhật nhà hàng)
    public class CategoryRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }  
}