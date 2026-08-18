using Oceanic_Horizon_Travel.DTOs.ReviewDtos;

namespace Oceanic_Horizon_Travel.Services.ReviewServices
{
    public interface IReviewServices
    {
        // ── VİTRİN ──
        Task CreateAsync(CreateReviewDto createReviewDto);
        Task<List<ResultReviewDto>> GetApprovedByTourAsync(string tourId);
        Task<Dictionary<int, int>> GetRatingDistributionAsync(string tourId);

        // ── ADMİN ──
        Task<List<ResultReviewDto>> GetAllAsync(string? status = null);
        Task<int> GetPendingCountAsync();
        Task SetApprovalAsync(string id, bool isApproved);
        Task DeleteAsync(string id);
    }
}
