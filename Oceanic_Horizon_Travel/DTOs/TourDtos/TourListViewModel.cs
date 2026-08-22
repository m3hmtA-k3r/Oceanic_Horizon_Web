using Oceanic_Horizon_Travel.DTOs.CategoryDtos;
using Oceanic_Horizon_Travel.DTOs.DestinationDtos;

namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    public class TourListViewModel
    {
        public List<ResultTourDto> Tours { get; set; } = new();

        // filtre panelini doldurmak için
        public List<ResultDestinationDto> Destinations { get; set; } = new();
        public List<ResultCategoryDto> Categories { get; set; } = new();

        public TourFilterDto Filter { get; set; } = new();

        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
