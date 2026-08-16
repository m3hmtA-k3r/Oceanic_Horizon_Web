using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Destination: BaseEntity //Destination  : Kullanıcıların sitede incelediği alan olacak
    {
        public LocalizedText Country { get; set; }
        public LocalizedText City { get; set; }
        public string SeoUrl { get; set; }       
        public LocalizedText ShortDescription { get; set; }
        public LocalizedText Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<ImageItem> Gallery { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }



    }
}
