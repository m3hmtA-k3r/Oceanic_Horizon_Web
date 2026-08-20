using MongoDB.Driver;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.BookingServices;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.PaymentServices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMongoCollection<Payment> _paymentCollection;
        private readonly IBookingServices _bookingServices;

        public PaymentServices(IDatabaseSettings databaseSettings, IBookingServices bookingServices)
        {
            _bookingServices = bookingServices;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _paymentCollection = database.GetCollection<Payment>(databaseSettings.PaymentCollectionName);
        }

        public async Task<string> CreateAsync(string bookingId, decimal amount, string paymentMethod)
        {
            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                TransactionNumber = $"TRX-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}",
                Status = "Başarılı",
                CreatedDate = DateTime.UtcNow
            };

            await _paymentCollection.InsertOneAsync(payment);

            // Ödeme kaydı düştü → rezervasyonun ödeme durumu da değişmeli
            await _bookingServices.SetPaymentStatusAsync(bookingId, "Ödendi");

            return payment.TransactionNumber;
        }

        public async Task<List<Payment>> GetByBookingAsync(string bookingId)
        {
            return await _paymentCollection.Find(x => x.BookingId == bookingId).ToListAsync();
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _paymentCollection
                .Find(_ => true)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            var payments = await _paymentCollection.Find(x => x.Status == "Başarılı").ToListAsync();
            return payments.Sum(x => x.Amount);
        }
    }
}
