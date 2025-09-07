namespace DoAn_WebAPI.Models.DTOs
{
    public class RestaurantRatingDTO
    {
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public string LogoUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
