using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Banner: BaseEntity // Burada Banner kontrol etmek için BaseEntity sınıfını ilişkilndirip kullnıyoruz.
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
