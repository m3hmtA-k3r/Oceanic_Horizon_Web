using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Room : BaseEntity
    {
        public string CurrencyType { get; set; }   // Para birimi — "TRY", "EUR"
        public string EstateId { get; set; } // oda hangi Mülke ait olacak 
        public string Name { get; set; } // Oda tipi adı -- Ana yol manzaralı gibi E5 otoyolunu net görüyorsun. 
        public int Capacity { get; set; } // Kaç kişi kalacak. 
        public decimal BasePrice { get; set; }  // Gecelik fiyat
        public string ThumbnailUrl { get; set; } // Oda gorseli 
        public bool IsActive { get; set; }  // Oda rezarvasyona uygundmu ? Degil ise listelenmez, rezerve edilemez

        public List<ImageItem> Images { get; set; }   // Oda görselleri galerisi

    }
}
