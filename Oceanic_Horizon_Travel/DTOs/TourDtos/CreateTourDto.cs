using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    public class CreateTourDto
    {
        public string? DestinationId { get; set; }
        public string? CategoryId { get; set; }


        public LocalizedText Title { get; set; } = new();
        public string? SeoUrl { get; set; }
        public LocalizedText ShortDescription { get; set; } = new();
        public LocalizedText Description { get; set; } = new();
        public LocalizedText Route { get; set; } = new();


        public int Day { get; set; }
        public int Night { get; set; }
        public int MaxCapacity { get; set; }
        public int MinParticipant { get; set; }


        public decimal BasePrice { get; set; }
        public string? CurrencyType { get; set; }


        public string? TourType { get; set; }
        public LocalizedText StartCity { get; set; } = new();
        public LocalizedText Transportation { get; set; } = new();
        public LocalizedText Accommodation { get; set; } = new();
        public string? GuideLanguage { get; set; }
        public LocalizedText VisaInfo { get; set; } = new();


        public List<LocalizedText> Included { get; set; } = new();
        public List<LocalizedText> Excluded { get; set; } = new();
        public List<string> Badges { get; set; } = new();
        public List<ImageItem> Images { get; set; } = new();
        public List<Amenity> Amenities { get; set; } = new();
        public List<TourDate> TourDates { get; set; } = new();
        public List<ItineraryDay> Itinerary { get; set; } = new();


        public string? ThumbnailUrl { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
