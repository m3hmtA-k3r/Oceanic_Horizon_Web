namespace Oceanic_Horizon_Travel.DTOs.QuestionDtos
{
    public class CreateQuestionDto
    {
        public string? TourId { get; set; }
        public string? MemberId { get; set; }
        public string? Text { get; set; }
    }
}
