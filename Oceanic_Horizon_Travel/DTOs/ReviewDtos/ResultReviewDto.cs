namespace Oceanic_Horizon_Travel.DTOs.ReviewDtos
{
    public class ResultReviewDto
    {
        public string? Id { get; set; }

        public string? MemberId { get; set; }
        public string? MemberName { get; set; }// servis dolduruyor

        public string? Type { get; set; }
        public string? EntityId { get; set; }
        public string? EntityTitle { get; set; }// servis dolduruyor — tur adı

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
