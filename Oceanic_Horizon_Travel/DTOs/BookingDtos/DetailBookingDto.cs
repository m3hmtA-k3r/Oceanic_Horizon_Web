using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.BookingDtos
{
    public class DetailBookingDto
    {
        public string? Id { get; set; }
        public string? BookingNumber { get; set; }

        public string? MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? MemberEmail { get; set; }
        public string? MemberPhone { get; set; }

        public string? TourId { get; set; }
        public string? TourTitle { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }

        public DateTime BookingDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal TotalPrice { get; set; }

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public List<BookingItem> Items { get; set; } = new();
        public List<Guest> Guests { get; set; } = new();
    }
}
