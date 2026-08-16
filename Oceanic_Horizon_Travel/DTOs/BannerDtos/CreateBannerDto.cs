using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.BannerDtos
{
    public class CreateBannerDto
    {
        public string? ImageUrl { get; set; }
        public LocalizedText Title { get; set; } = new();
        public LocalizedText Description { get; set; } = new();
        public bool IsActive { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
