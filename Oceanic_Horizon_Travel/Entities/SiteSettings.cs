using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    // Sitenin kurumsal bilgileri. 
    public class SiteSettings : BaseEntity
    {
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public LocalizedText About { get; set; }
        public LocalizedText Mission { get; set; }
        public LocalizedText Vision { get; set; }

        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }

     
    }
}
