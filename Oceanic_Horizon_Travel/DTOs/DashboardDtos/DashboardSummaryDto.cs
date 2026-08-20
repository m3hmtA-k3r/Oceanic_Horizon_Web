namespace Oceanic_Horizon_Travel.DTOs.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int TotalMembers { get; set; }

        public int PendingReviews { get; set; }
        public int PendingQuestions { get; set; }
    }
}
