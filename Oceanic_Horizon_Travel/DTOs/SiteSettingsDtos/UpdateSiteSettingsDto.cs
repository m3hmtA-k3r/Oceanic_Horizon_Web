using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos
{
    public class UpdateSiteSettingsDto
    {
        public string? Id { get; set; }

        public string? CompanyName { get; set; }
        public string? LogoUrl { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public LocalizedText About { get; set; } = new();
        public LocalizedText Mission { get; set; } = new();
        public LocalizedText Vision { get; set; } = new();

        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }
        public string? LinkedIn { get; set; }

        public IFormFile? LogoFile { get; set; }
    }
}
