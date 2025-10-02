namespace Layout_Client.Model.DTO
{
    public class MenuItemRatingDTO
    {
        public int MenuItemID { get; set; }
        public string MenuItemName { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
    }
}