using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.BookingDtos
{
    public class CreateBookingDto //Önyüzden gelecek rezarvasyon talebi
    {
        public string? MemberId { get; set; }   // controller claim useri dolduracak...

        public string? TourId { get; set; }
        public string? TourDateId { get; set; }

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public List<Guest> Guests { get; set; } = new();
    }
}
