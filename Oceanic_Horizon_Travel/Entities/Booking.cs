using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Booking : BaseEntity 
        //Booking = rezervasyon fişi. Bir üyenin "şunu, şu tarihte, şu kadar kişi için, şu fiyata rezerve ettim" kaydı. Alışverişteki sipariş kaydının karşılığı.
    {
        public string BookingNumber { get; set; }
        public string MemberId { get; set; }
        public string HostId { get; set; }   // Kazancın gideceği ev sahibi — Wallet'ı bu kişiye ait
        public DateTime BookingDate { get; set; } // Rezervasyonun yapıldığı tarih
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public decimal CommissionAmount { get; set; }   // Acentenin aldığı komisyon
        public decimal HostEarning { get; set; }// Ev sahibine kalan tutar
        public decimal TotalPrice { get; set; }

        public List<BookingItem> Items { get; set; }//tur ve/veya konaklama
        public List<Guest> Guests { get; set; }// Seyahat edecek kişiler

    }
}
