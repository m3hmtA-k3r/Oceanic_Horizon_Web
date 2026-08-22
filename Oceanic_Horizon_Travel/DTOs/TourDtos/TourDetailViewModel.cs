using Oceanic_Horizon_Travel.DTOs.QuestionDtos;
using Oceanic_Horizon_Travel.DTOs.ReviewDtos;

namespace Oceanic_Horizon_Travel.DTOs.TourDtos
{
    public class TourDetailViewModel
    {
        public ResultTourDto Tour { get; set; } = new();

        public string DestinationName { get; set; } = "";
        public string CategoryName { get; set; } = "";

        public List<ResultReviewDto> Reviews { get; set; } = new();
        public Dictionary<int, int> RatingDistribution { get; set; } = new();

        public List<ResultQuestionDto> Questions { get; set; } = new();
    }
}
