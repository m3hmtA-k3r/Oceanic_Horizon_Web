using Oceanic_Horizon_Travel.DTOs.BookingDtos;

namespace Oceanic_Horizon_Travel.DTOs.DashboardDtos
{
    public class DashboardViewModel
    {
        public DashboardSummaryDto Summary { get; set; } = new();

        public List<ChartPointDto> MonthlyRevenue { get; set; } = new();
        public List<ChartPointDto> BookingStatus { get; set; } = new();
        public List<ChartPointDto> TopTours { get; set; } = new();

        public List<ResultBookingDto> RecentBookings { get; set; } = new();
    }
}
