using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.BookingDtos
{
    public class BookingCreateViewModel
    {
        public string TourId { get; set; } = "";
        public string TourDateId { get; set; } = "";

        
        public string TourTitle { get; set; } = ""; // ekranda gösterilecek özet — formdan geri gelmiyor, sunucu yeniden dolduruyor
        public string? ThumbnailUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? CurrencyType { get; set; }
        public int AvailableSeats { get; set; }
        public int Day { get; set; }
        public int Night { get; set; }

        
        public int AdultCount { get; set; } = 1; // kullanıcının girdiği
        public int ChildCount { get; set; }
        public List<Guest> Guests { get; set; } = new();
    }
}
