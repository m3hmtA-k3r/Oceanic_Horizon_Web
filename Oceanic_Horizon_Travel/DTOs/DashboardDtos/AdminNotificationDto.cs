namespace Oceanic_Horizon_Travel.DTOs.DashboardDtos
{
    
    public class AdminNotificationDto
    {// Zil menüsü her sayfada çiziliyor, hafif tutuldu
        public int PendingBookings { get; set; }
        public int PendingReviews { get; set; }
        public int PendingQuestions { get; set; }

        public int Total => PendingBookings + PendingReviews + PendingQuestions;
    }
}
