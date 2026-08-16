using Oceanic_Horizon_Travel.DTOs.DestinationDtos;

namespace Oceanic_Horizon_Travel.Services.DestinationServices
{
    public interface IDestinationServices
    {
        Task<List<ResultDestinationDto>> GetAllAsync();
        Task<ResultDestinationDto> GetByIdAsync(string id);
        Task CreateAsync(CreateDestinationDto createDestinationDto);
        Task UpdateAsync(UpdateDestinationDto updateDestinationDto);
        Task DeleteAsync(string id);
    }
}
