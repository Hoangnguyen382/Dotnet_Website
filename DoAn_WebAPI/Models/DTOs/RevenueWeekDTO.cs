
namespace DoAn_WebAPI.Models.DTOs
{
    public class RevenueWeekDTO
    {
        public string DayOfWeek { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Revenue { get; set; }
    }
}
