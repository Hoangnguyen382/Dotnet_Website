namespace Layout_Admin.Model.DTO
{
    public class RestaurantRequestDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OpeningHours { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
    }
    public class RestaurantResponseDTO
    {
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OpeningHours { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
    }
}


