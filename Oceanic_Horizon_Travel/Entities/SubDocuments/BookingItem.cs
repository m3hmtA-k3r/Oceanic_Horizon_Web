namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    public class BookingItem
    {
        public string Type { get; set; } // "Tour" ve "Estate"

        public string Title { get; set; } // Ürün adı — rezervasyon anında KOPYALANIR

        public string? TourDepartureId { get; set; }// Tur kalemiyse hangi kalkış

        public string? RoomId { get; set; } // Konaklama kalemiyse hangi oda

        public DateTime? CheckIn { get; set; } // Konaklama girişi

        public DateTime? CheckOut { get; set; }// Konaklama çıkışı

        public int Quantity { get; set; } // Kişi sayısı (tur) veya oda sayısı (konaklama)

        public decimal UnitPrice { get; set; }  // Birim fiyat — rezervasyon anında DONMUŞ

        public decimal Subtotal { get; set; }// Ara toplam
    }
}
