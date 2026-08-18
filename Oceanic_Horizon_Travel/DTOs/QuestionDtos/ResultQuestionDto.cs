namespace Oceanic_Horizon_Travel.DTOs.QuestionDtos
{
    public class ResultQuestionDto
    {
        public string? Id { get; set; }

        public string? TourId { get; set; }
        public string? TourTitle { get; set; }        // servis dolduruyor — toplu sorgu ile

        public string? MemberId { get; set; }
        public string? MemberName { get; set; }       // servis dolduruyor

        public string? Text { get; set; }
        public string? Answer { get; set; }
        public DateTime? AnsweredDate { get; set; }

        public bool IsAnswered { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
