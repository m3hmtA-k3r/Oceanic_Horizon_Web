using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class SiteSettings: BaseEntity
    {
        public string CompanyName { get; set; } // Firma adı — "Oceanic Horizon Travel"
        public decimal CommissionRate { get; set; }   // Varsayılan komisyon oranı — yüzde olarak, 15 = %15
        public string? LogoUrl { get; set; } // Logo görseli

        public string? Phone { get; set; }  // İletişim telefonu

        public string? Email { get; set; } 

        public string? Address { get; set; } 

        public string? About { get; set; } 

        public string? Mission { get; set; }  // Misyon metni

        public string? Vision { get; set; } // Vizyon metni

        public string? Facebook { get; set; }   

        public string? Instagram { get; set; }     

        public string? Youtube { get; set; } 

        public string? LinkedIn { get; set; }  
    }
}
