namespace Oceanic_Horizon_Travel.DTOs.BookingDtos
{
    
    public class ResultBookingDto
    {
        public string? Id { get; set; }
        public string? BookingNumber { get; set; }

        public string? MemberId { get; set; }
        public string? MemberName { get; set; }

        public string? TourId { get; set; }
        public string? TourTitle { get; set; }   
        public DateTime BookingDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal TotalPrice { get; set; }

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public List<Entities.SubDocuments.Guest> Guests { get; set; } = new(); //accordion card için düşündüm bunu

    }
}
