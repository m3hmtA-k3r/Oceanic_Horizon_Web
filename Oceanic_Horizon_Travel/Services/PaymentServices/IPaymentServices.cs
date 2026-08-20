using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<string> CreateAsync(string bookingId, decimal amount, string paymentMethod);  
        Task<List<Payment>> GetByBookingAsync(string bookingId);
        Task<List<Payment>> GetAllAsync();
        Task<decimal> GetTotalRevenueAsync();   // Dashboard'da kullanılacak
    }
}
