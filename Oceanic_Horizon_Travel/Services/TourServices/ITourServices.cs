using Oceanic_Horizon_Travel.DTOs.TourDtos;

namespace Oceanic_Horizon_Travel.Services.TourServices
{
    public interface ITourServices
    {
        Task<List<ResultTourDto>> GetAllAsync();
        Task<ResultTourDto> GetByIDAsync(string id);
        Task CreateAsync(CreateTourDto createTourDto);
        Task UpdateAsync(UpdateTourDto updateTourDto);
        Task DeleteAsync(string id);
    }
}
