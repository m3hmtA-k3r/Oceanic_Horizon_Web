using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Tour : BaseEntity // Satışa sunulan round paketi — "Sofia nın Rüyası, 5 gece, €1.250". Belirli tarihleri TourDeparture tutar.
    {
        public string DestinationId { get; set; } // Hangi destinasyona ait
        public string Title { get; set; } // Tur adi
        public string SeoUrl { get; set; } // Adress link adi oluyor/ 
        public string Description { get; set; } // Tur Tanitim metni
        public int Night {  get; set; }  // ka. gece olacak 
        public decimal BasePrice { get; set; } //baslangic fiyati
        public string CurrencyType { get; set; } // Para birimi Artik Allah ne verirse/ 
        public double Rating { get; set; } // Ortalama Puan
        public int ReviewCount { get; set; } // Ka. kisi yorum yapti
        public string ThumbnailUrl { get; set; } // Kapak gorseli
        public bool IsFeatured { get; set; } // Ana sayfada one ciksin mi
        public bool IsActive { get; set; } // Aktif mi ? 

        public List<ImageItem> Images { get; set; }      // Tur görseller için galerisi

        public List<Amenity> Amenities { get; set; }     // Tur olanakları — Uçuş, Otel, Yat

    }
}
