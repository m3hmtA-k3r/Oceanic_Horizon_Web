namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    
    public class TourFilterDto
    {// önyüz filtre paneli 
        public string? Q { get; set; } // metin arama
        public string? DestinationId { get; set; }
        public string? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? MinDay { get; set; }
        public int? MaxDay { get; set; }

        public string? Sort { get; set; }  // price-asc · price-desc · rating
        public int Page { get; set; } = 1;
    }
}
