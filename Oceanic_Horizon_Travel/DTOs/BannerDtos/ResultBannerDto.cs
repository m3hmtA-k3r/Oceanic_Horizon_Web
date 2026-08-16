using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.BannerDtos
{
    public class ResultBannerDto
    {
        public string? Id { get; set; }
        public string? ImageUrl { get; set; }
        public LocalizedText Title { get; set; } = new();
        public LocalizedText Description { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
