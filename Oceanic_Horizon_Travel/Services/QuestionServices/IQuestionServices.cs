using Oceanic_Horizon_Travel.DTOs.QuestionDtos;

namespace Oceanic_Horizon_Travel.Services.QuestionServices
{
    public interface IQuestionServices
    {
        Task CreateAsync(CreateQuestionDto createQuestionDto);
        Task<List<ResultQuestionDto>> GetApprovedByTourAsync(string tourId);

        //Admin
        Task<List<ResultQuestionDto>> GetAllAsync(string? status = null);
        Task<ResultQuestionDto?> GetByIdAsync(string id);
        Task<int> GetPendingCountAsync();
        Task AnswerAsync(AnswerQuestionDto answerQuestionDto, string adminId);
        Task SetApprovalAsync(string id,bool isApproved);
        Task DeleteAsync(string id);
    }
}
