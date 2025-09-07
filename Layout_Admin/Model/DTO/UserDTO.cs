namespace Layout_Admin.Model.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Staff";
    }
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public int RestaurantID { get; set; } 
        public string Email { get; set; } = "";
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}