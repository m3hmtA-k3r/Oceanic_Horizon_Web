using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Category: BaseEntity// Tur kategorisi"Kültür-Doğa&Macera
    {
        public LocalizedText Name { get; set; }
        public string SeoUrl { get; set; }        // tüm dillerde aynı
        public string Icon { get; set; }         // Material Symbols ikon adı
        public bool IsActive { get; set; }
    }
}
