using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    public class ResultTourDto
    {
        public string? Id { get; set; }
        public string? DestinationId { get; set; }
        public LocalizedText Title { get; set; } = new();
        public string? SeoUrl { get; set; }
        public LocalizedText Description { get; set; } = new();
        public int Night { get; set; }
        public decimal BasePrice { get; set; }
        public string? CurrencyType { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
    }
}
