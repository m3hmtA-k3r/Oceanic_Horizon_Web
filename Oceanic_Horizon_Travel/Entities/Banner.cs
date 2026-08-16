using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Banner: BaseEntity // Burada Banner kontrol etmek için BaseEntity sınıfını ilişkilndirip kullnıyoruz.
    {
        public string ImageUrl { get; set; }
        public LocalizedText Title { get; set; }
        public LocalizedText Description { get; set; }
        public bool IsActive { get; set; }

    }
}
