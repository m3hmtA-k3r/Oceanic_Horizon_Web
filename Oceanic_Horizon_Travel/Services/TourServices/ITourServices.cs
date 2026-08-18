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

        Task<List<ResultTourDto>> GetByIdsAsync(List<string> ids); // Toplu çekme metotları 

        Task UpdateRatingAsync(string tourId, double rating, int reviewCount); //puan güncelleme
        Task UpdateSeatsAsync(string tourId, string tourDateId, int delta); // kalkış kontenjanını negatif düsürür, pozitif iade eder



    }
}
