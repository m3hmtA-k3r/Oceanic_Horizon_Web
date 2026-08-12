using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{

    // Payout = ev sahibine yapılan ödeme. rezervasyonlardan biriken HostEarning tutarları
    // dönem dönem toplanıp ev sahibinin IBAN'ına aktarılır. Bu kayıt o aktarımın makbuzudur.
    public class Payout: BaseEntity
    {
        public string HostId { get; set; }  // Ödeme kime yapıldı — Member kimliği
        public decimal Amount { get; set; } // Ödenen Toplam Tutar
        public string Status { get; set; } // Pending + Ödendi + İptal
        public DateTime? PaidDate { get; set; }   //Ne zaman ödendi - Ödenene kadar boş kalacak
    }
}
