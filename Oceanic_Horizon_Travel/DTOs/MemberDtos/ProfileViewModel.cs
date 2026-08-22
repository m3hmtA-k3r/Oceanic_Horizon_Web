using Oceanic_Horizon_Travel.DTOs.BookingDtos;

namespace Oceanic_Horizon_Travel.DTOs.MemberDtos
{
    public class ProfileViewModel
    {
        public ResultMemberDto Member { get; set; }
        public List<ResultBookingDto> Bookings { get; set; } = new();

        //burda 3 sayaç olarak yaptım
        public int TotalBookings => Bookings.Count;
        public int ActiveBookings => Bookings.Count(x => x.Status == "Onaylandı");
        public decimal TotalSpent => Bookings.Where(x => x.PaymentStatus == "Ödendi").Sum(x => x.TotalPrice);
    }
}
