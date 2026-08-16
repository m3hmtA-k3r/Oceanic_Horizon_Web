using Oceanic_Horizon_Travel.DTOs.BannerDtos;

namespace Oceanic_Horizon_Travel.Services.BannerServices
{
    public interface IBannerServices
    {
        Task<List<ResultBannerDto>> GetAllAsync();
        Task<ResultBannerDto> GetByIdAsync(string id);
        Task CreateAsync(CreateBannerDto createBannerDto);
        Task UpdateAsync(UpdateBannerDto updateBannerDto);
        Task DeleteAsync(string id);
    }
}
