using Oceanic_Horizon_Travel.Entities.Common;
using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.Entities
{
    // Satışa sunulan tur paketi. Tarihler ve gün gün program bu belgenin İÇİNDE tutulur.
    public class Tour : BaseEntity
    {
        public LocalizedText Title { get; set; }
        public string SeoUrl { get; set; }
        public LocalizedText ShortDescription { get; set; }
        public LocalizedText Description { get; set; }
        public LocalizedText Route { get; set; } 


        public int Day { get; set; }
        public int Night { get; set; }
        public int MaxCapacity { get; set; }  
        public int MinParticipant { get; set; } 


        public decimal BasePrice { get; set; }     
        public string CurrencyType { get; set; }


        public string TourType { get; set; }   
        public LocalizedText StartCity { get; set; }   
        public LocalizedText Transportation { get; set; }    
        public LocalizedText Accommodation { get; set; }  
        public string GuideLanguage { get; set; }    
        public LocalizedText VisaInfo { get; set; }   


        public List<LocalizedText> Included { get; set; }  
        public List<LocalizedText> Excluded { get; set; } 
        public List<string> Badges { get; set; }    
        public List<ImageItem> Images { get; set; }  
        public List<Amenity> Amenities { get; set; }
        public List<TourDate> TourDates { get; set; }          
        public List<ItineraryDay> Itinerary { get; set; }      


        public double Rating { get; set; }
        public int ReviewCount { get; set; }


        public string ThumbnailUrl { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }


        public string DestinationId { get; set; }
        public string CategoryId { get; set; }
    }
}
