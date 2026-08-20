using Oceanic_Horizon_Travel.DTOs.BookingDtos;

namespace Oceanic_Horizon_Travel.Services.BookingServices
{
    public interface IBookingServices
    {
        //ön taraf için
        Task<string> CreateAsync(CreateBookingDto createBookingDto);  
        Task<List<ResultBookingDto>> GetByMemberAsync(string memberId);



        
        // Admin için
        Task<List<ResultBookingDto>> GetAllAsync(string? status = null);
        Task<DetailBookingDto?> GetDetailAsync(string id);
        Task SetStatusAsync(string id, string status);
        Task SetPaymentStatusAsync(string id, string paymentStatus);
        Task<int> GetPendingCountAsync();

        //Search için
        Task<List<ResultBookingDto>> SearchAsync(string term);   // rezervasyon numarası

    }
}
