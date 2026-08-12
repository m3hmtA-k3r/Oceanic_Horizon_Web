using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Estate: BaseEntity
    {
        public string DestinationId { get; set; } // Mülkün bulunduğu destinasyonun Id'si
        public string Name { get; set; } //İlan adı — otel, ev veya villa adı
        public string SeoUrl { get; set; } // Mülk Adres çubuğundaki ad  URL'si
        public string OwnerId { get; set; }   // İlanı ekleyen ev sahibinin (Host) Member kimliği
        public int Star { get; set; } // Otelin yıldız sayısı 1-5 arasında bıkacaz .
        public string Description { get; set; } 
        public string Address { get; set; } 
        public double Rating { get; set; } // Ortalama puan — 4.9
        public int ReviewCount { get; set; }   // Kaç yorum yapıldı
        public string ThumbnailUrl { get; set; } // Kapak resim URL'si
        public bool IsApproved { get; set; }  // Admin onayladı mı — onaysız ilan sitede görünmez
        public bool IsFeatured { get; set; } // Sayfada öne çıkarılsın mı ? 
        public bool IsActive { get; set; }

        public string Type { get; set; } // İlan türü — "Hotel", "House", "Villa", "Apartment"

        public List<ImageItem> Images { get; set; } 

        public List<Amenity> Amenities { get; set; } 

    }
}
