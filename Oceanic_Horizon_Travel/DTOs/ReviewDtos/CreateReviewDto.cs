namespace Oceanic_Horizon_Travel.DTOs.ReviewDtos
{
    public class CreateReviewDto
    {
        public string? MemberId { get; set; }
        public string? Type { get; set; }        // "Tour"
        public string? EntityId { get; set; }    // TourId
        public int Rating { get; set; }          // 1-5
        public string? Comment { get; set; }
    }
}
