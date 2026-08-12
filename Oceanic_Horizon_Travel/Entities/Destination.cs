using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Destination: BaseEntity //Destination  : Kullanıcıların sitede incelediği alan olacak
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string SeoUrl { get; set; } // Adres çubuğunun sonundaki url kısmı. Örn: oceanichorizontravel.com/destination/istanbul gibi görünecek inşallah:)
        public string ShortDescription { get; set; } // Kısa açıklama. Örn: "İstanbul, Türkiye'nin en büyük ve en kalabalık şehridir."

        public string Description { get; set; } 

        public string ThumbnailUrl { get; set; } // Kapak görselinin adresi
        public List<ImageItem> Gallery { get; set; }   // Destinasyon galerisi

        public bool IsFeatured { get; set; } // Ana sayfada öne çıkarayımmı
    
        public bool IsActive { get; set; } // Aktif mi değil mi. Admin panelinde aktif olmayanlar listelenmeyecek.



    }
}
