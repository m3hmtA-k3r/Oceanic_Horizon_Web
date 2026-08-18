using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    // Rezervasyon İki satış alanına birden hizmet vermelidir
    public class Booking : BaseEntity
    {
        
        public string BookingNumber { get; set; }
        public string MemberId { get; set; } // rezervasyonu yapan üye
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }// "Bekliyor" · "Onaylandı" · "İptal Edildi"
        public string PaymentStatus { get; set; }
        public decimal TotalPrice { get; set; }

    
        public string TourId { get; set; }
        public string TourDateId { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        
        public string HostId { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal HostEarning { get; set; }

        
        public List<BookingItem> Items { get; set; }
        public List<Guest> Guests { get; set; }
    }
}
