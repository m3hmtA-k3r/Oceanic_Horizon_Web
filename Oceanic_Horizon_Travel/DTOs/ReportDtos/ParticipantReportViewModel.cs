using Oceanic_Horizon_Travel.DTOs.TourDtos;

namespace Oceanic_Horizon_Travel.DTOs.ReportDtos
{
    public class ParticipantReportViewModel
    {
        public List<ResultTourDto> Tours { get; set; } = new();   // açılır liste için

        public string? SelectedTourDateId { get; set; }

        public string TourTitle { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<ParticipantDto> Participants { get; set; } = new();
    }
}
