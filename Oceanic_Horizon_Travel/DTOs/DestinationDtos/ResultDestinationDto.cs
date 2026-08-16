using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.DestinationDtos
{
    public class ResultDestinationDto
    {
        public string? Id { get; set; }
        public LocalizedText Country { get; set; } = new();
        public LocalizedText City { get; set; } = new();
        public string? SeoUrl { get; set; }
        public LocalizedText ShortDescription { get; set; } = new();
        public LocalizedText Description { get; set; } = new();
        public string? ThumbnailUrl { get; set; }
        public List<ImageItem> Gallery { get; set; } = new();
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
    }
}
