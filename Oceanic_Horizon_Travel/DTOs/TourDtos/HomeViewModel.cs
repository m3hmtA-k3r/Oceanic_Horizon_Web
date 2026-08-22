using Oceanic_Horizon_Travel.DTOs.DestinationDtos;
using Oceanic_Horizon_Travel.DTOs.ReviewDtos;

namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    public class HomeViewModel
    {
        public List<ResultTourDto> FeaturedTours { get; set; } = new();
        public List<ResultDestinationDto> Destinations { get; set; } = new();
        public List<ResultReviewDto> Reviews { get; set; } = new();

        public int TotalTours { get; set; }
        public int TotalDestinations { get; set; }
        public int TotalMembers { get; set; }
    }
}
