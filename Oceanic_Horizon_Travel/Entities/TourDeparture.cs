using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{   // TourDeparture = turun SATILAN TArihi. Tour "Sofianin Rüyası" ürününü tanımlar,
    // bu kayıt "15-20 Haziran kalkışı, 20 kişilik, €1.350" der.
    // Aynı tur yaz boyunca 10 kez kalkar; her kalkışın kendi kontenjanı ve fiyatı olur.
    public class TourDeparture: BaseEntity
    {
        public string TourId { get; set; } // Hangi Tura ait
        public DateTime DepartureDate { get; set; } // Kalkis tarihi
        public DateTime ReturnDate { get; set; } // Donus tarihi
        public int TotalSeats {  get; set; } // Toplam kontenjan — 20 kişilik
        public int AvailableSeats { get; set; } // Kalan bos yer rezarvasyon geldikce duser
        public decimal Price  { get; set; }// Bu kalkışa özel kişi başı fiyat
        public string Status { get; set; } // Kalkiş durumu acik doldu veya iptal edildi gibi

    }
}
