using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.DTOs.DashboardDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;
using System.Globalization;

namespace Oceanic_Horizon_Travel.Services.DashboardServices
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<Member> _memberCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<Question> _questionCollection;

        private readonly ITourServices _tourServices;
        private readonly IMemberServices _memberServices;

        public DashboardServices(IDatabaseSettings databaseSetting, ITourServices tourServices, IMemberServices memberServices)
        {
            _tourServices = tourServices;
            _memberServices = memberServices;

            var client = new MongoClient(databaseSetting.ConnectionString);
            var database = client.GetDatabase(databaseSetting.DatabaseName);

            _bookingCollection = database.GetCollection<Booking>(databaseSetting.BookingCollectionName);
            _memberCollection = database.GetCollection<Member>(databaseSetting.MemberCollectionName);
            _reviewCollection = database.GetCollection<Review>(databaseSetting.ReviewCollectionName);
            _questionCollection = database.GetCollection<Question>(databaseSetting.QuestionCollectionName);
        }
        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            return new DashboardViewModel
            {
                Summary = await GetSummaryAsync(),
                MonthlyRevenue = await GetMonthlyRevenueAsync(),
                BookingStatus = await GetBookingStatusAsync(),
                TopTours = await GetTopToursAsync(),
                RecentBookings = await GetRecentBookingsAsync(),
            };
        }

        // Layout'ta her sayfada çağrılıyor — sadece üç sayaç, Aggregation yok
        public async Task<AdminNotificationDto> GetNotificationsAsync()
        {
            return new AdminNotificationDto
            {
                PendingBookings = (int)await _bookingCollection.CountDocumentsAsync(x => x.Status == "Bekliyor"),
                PendingReviews = (int)await _reviewCollection.CountDocumentsAsync(x => !x.IsApproved),
                PendingQuestions = (int)await _questionCollection.CountDocumentsAsync(x => !x.IsAnswered)
            };
        }


        private async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // Ciro: AGGREGATION — ödenmiş özet rezervasyonları veritabanı seviyesinde toplama

            var revenueResult = await _bookingCollection.Aggregate()
                .Match(x => x.PaymentStatus == "Ödendi")
                .Group(x => 1, g => new { Total = g.Sum(x => x.TotalPrice) })
                .FirstOrDefaultAsync();

            return new DashboardSummaryDto
            {
                TotalRevenue = revenueResult?.Total ?? 0,

                TotalBookings = (int)await _bookingCollection.CountDocumentsAsync(_ => true),
                PendingBookings = (int)await _bookingCollection.CountDocumentsAsync(x => x.Status == "Bekliyor"),
                TotalMembers = (int)await _memberCollection.CountDocumentsAsync(_ => true),

                PendingReviews = (int)await _reviewCollection.CountDocumentsAsync(x => !x.IsApproved),
                PendingQuestions = (int)await _questionCollection.CountDocumentsAsync(x => !x.IsAnswered)
            };
        }

        //Son 6 Aylık Ciro Trendi — GRAFİKLER 
        private async Task<List<ChartPointDto>> GetMonthlyRevenueAsync()
        {
            var today = DateTime.UtcNow;
            var start = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5);

            var grouped = await _bookingCollection.Aggregate()
                .Match(x => x.PaymentStatus == "Ödendi" && x.BookingDate >= start)
                .Group(
                    x => new { x.BookingDate.Year, x.BookingDate.Month },
                    g => new { g.Key.Year, g.Key.Month, Total = g.Sum(x => x.TotalPrice) })
                .ToListAsync();

            var culture = new CultureInfo("tr-TR");
            var result = new List<ChartPointDto>();

            // Boş ayları da göster — grafik kesintisiz olmalı
            for (int i = 0; i < 6; i++)
            {
                var month = start.AddMonths(i);
                var match = grouped.FirstOrDefault(x => x.Year == month.Year && x.Month == month.Month);

                result.Add(new ChartPointDto
                {
                    Label = month.ToString("MMM yyyy", culture),
                    Value = match?.Total ?? 0
                });
            }

            return result;
        }

        // Rezervasyon durum dağılımı
        private async Task<List<ChartPointDto>> GetBookingStatusAsync()
        {
            var grouped = await _bookingCollection.Aggregate()
                .Group(x => x.Status, g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return grouped
                .Select(x => new ChartPointDto { Label = x.Status, Value = x.Count })
                .OrderByDescending(x => x.Value)
                .ToList();
        }

        // En çok satan 5 tur 
        private async Task<List<ChartPointDto>> GetTopToursAsync()
        {
            var grouped = await _bookingCollection.Aggregate()
                .Match(x => x.Status != "İptal Edildi" && x.TourId != null)
                .Group(x => x.TourId, g => new { TourId = g.Key, Count = g.Count() })
                .SortByDescending(x => x.Count)
                .Limit(5)
                .ToListAsync();

            if (grouped.Count == 0) return new List<ChartPointDto>();

            // Tur adlarını toplu çek — N+1 önlemi
            var tourIds = grouped.Select(x => x.TourId!).ToList();
            var tours = await _tourServices.GetByIdsAsync(tourIds);
            var tourMap = tours.ToDictionary(x => x.Id!, x => x.Title.Tr ?? "");

            return grouped
                .Select(x => new ChartPointDto
                {
                    Label = tourMap.GetValueOrDefault(x.TourId!, "—"),
                    Value = x.Count
                })
                .ToList();
        }

        private async Task<List<ResultBookingDto>> GetRecentBookingsAsync()
        {//Son 5 Rezervasyonu beslen method
            var bookings = await _bookingCollection
                .Find(_ => true)
                .SortByDescending(x => x.BookingDate)
                .Limit(5)
                .ToListAsync();

            if (bookings.Count == 0) return new List<ResultBookingDto>();

            var tourIds = bookings.Where(x => x.TourId != null).Select(x => x.TourId!).Distinct().ToList();
            var memberIds = bookings.Select(x => x.MemberId).Distinct().ToList();

            var tours = await _tourServices.GetByIdsAsync(tourIds);
            var members = await _memberServices.GetByIdsAsync(memberIds);

            var tourMap = tours.ToDictionary(x => x.Id!, x => x.Title.Tr ?? "");
            var memberMap = members.ToDictionary(x => x.Id!, x => $"{x.FirstName} {x.LastName}");

            return bookings.Select(b => new ResultBookingDto
            {
                Id = b.Id,
                BookingNumber = b.BookingNumber,
                MemberId = b.MemberId,
                MemberName = memberMap.GetValueOrDefault(b.MemberId ?? "", "—"),
                TourId = b.TourId,
                TourTitle = tourMap.GetValueOrDefault(b.TourId ?? "", "—"),
                BookingDate = b.BookingDate,
                Status = b.Status,
                PaymentStatus = b.PaymentStatus,
                TotalPrice = b.TotalPrice,
                AdultCount = b.AdultCount,
                ChildCount = b.ChildCount
            }).ToList();
        }
    }
}

