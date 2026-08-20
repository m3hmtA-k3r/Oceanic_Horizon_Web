using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.DTOs.TourDtos;

namespace Oceanic_Horizon_Travel.DTOs.SearchDtos
{
    public class SearchResultViewModel
    {
        public string Query { get; set; } = "";

        public List<ResultTourDto> Tours { get; set; } = new();
        public List<ResultBookingDto> Bookings { get; set; } = new();
        public List<ResultMemberDto> Members { get; set; } = new();

        public int TotalCount => Tours.Count + Bookings.Count + Members.Count;
    }
}
