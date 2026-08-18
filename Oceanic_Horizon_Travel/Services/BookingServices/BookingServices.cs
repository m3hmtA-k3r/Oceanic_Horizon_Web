using AutoMapper;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Entities.SubDocuments;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.BookingServices
{
    public class BookingServices : IBookingServices
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly ITourServices _tourServices;
        private readonly IMemberServices _memberServices;
        private readonly IMapper _mapper;

        public BookingServices(IDatabaseSettings databaseSettings, ITourServices tourServices, IMemberServices memberServices,IMapper mapper)
        {
            _mapper = mapper;
            _tourServices = tourServices;
            _memberServices = memberServices;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _bookingCollection = database.GetCollection<Booking>(databaseSettings.BookingCollectionName);
        }
        public async Task<string> CreateAsync(CreateBookingDto createBookingDto)
        {
            var tour = await _tourServices.GetByIDAsync(createBookingDto.TourId);

            if (tour == null)
                throw new InvalidOperationException("Tur bulunamadı.");

            var tourDate = tour.TourDates.FirstOrDefault(x => x.Id == createBookingDto.TourDateId);
            if (tourDate == null)
                throw new InvalidOperationException("Kalkış tarihi bulunamadı.");

            if (tourDate.Status != "Açık")
                throw new InvalidOperationException("Bu kalkış tarihi satışa kapalı.");

            var totalPeople = createBookingDto.AdultCount + createBookingDto.ChildCount;
            if (totalPeople <= 0)
                throw new InvalidOperationException("En az bir katılımcı seçmelisin.");

            if (tourDate.AvailableSeats < totalPeople)
                throw new InvalidOperationException($"Bu tarihte yalnızca {tourDate.AvailableSeats} koltuk kaldı.");

            // Fiyat SUNUCUDA hesaplanıyor — çocuk yarım ücret
            var adultTotal = createBookingDto.AdultCount * tourDate.Price;
            var childTotal = createBookingDto.ChildCount * tourDate.Price * 0.5m;
            var totalPrice = adultTotal + childTotal;

            var booking = new Booking
            {
                BookingNumber = GenerateBookingNumber(),
                MemberId = createBookingDto.MemberId!,
                BookingDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                Status = "Bekliyor",
                PaymentStatus = "Ödenmedi",
                TotalPrice = totalPrice,

                TourId = createBookingDto.TourId,
                TourDateId = createBookingDto.TourDateId,
                AdultCount = createBookingDto.AdultCount,
                ChildCount = createBookingDto.ChildCount,

                Guests = createBookingDto.Guests ?? new List<Guest>(),
                Items = new List<BookingItem>
                {
                    new BookingItem
                    {
                        Type = "Tour",
                        Title = tour.Title.Tr ?? "",
                        TourDateId = createBookingDto.TourDateId,
                        CheckIn = tourDate.StartDate,
                        CheckOut = tourDate.EndDate,
                        Quantity = totalPeople,
                        UnitPrice = tourDate.Price,
                        Subtotal = totalPrice
                    }
                }
            };

            await _bookingCollection.InsertOneAsync(booking);

            //  kayıt oluştuktan sonra Koltukları eksiye düş
            await _tourServices.UpdateSeatsAsync(createBookingDto.TourId, createBookingDto.TourDateId!, -totalPeople);

            return booking.BookingNumber;
        }

        public async Task<List<ResultBookingDto>> GetByMemberAsync(string memberId)
        {
            var bookings = await _bookingCollection.Find(x => x.MemberId == memberId)
                                                   .SortByDescending(x => x.BookingDate)
                                                   .ToListAsync();
            return await EnrichAsync(bookings);
        }




        public async Task<List<ResultBookingDto>> GetAllAsync(string? status = null)
        {
            var filter = string.IsNullOrWhiteSpace(status) ? Builders<Booking>.Filter.Empty : Builders<Booking>.Filter.Where(x => x.Status == status);

            var bookings = await _bookingCollection
                    .Find(filter)
                    .SortByDescending(x => x.BookingDate)
                    .ToListAsync();

            return await EnrichAsync(bookings);
        }

        public async Task<DetailBookingDto?> GetDetailAsync(string id)
        {
            var booking = await _bookingCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            if(booking == null)
                return null;

            var detail = _mapper.Map<DetailBookingDto>(booking);
            var member = await _memberServices.GetByIdAsync(booking.MemberId);
            if(member != null)
            {
                detail.MemberName = $"{member.FirstName} {member.LastName}";
                detail.MemberEmail = member.Email;
                detail.MemberPhone = member.PhoneNumber;

                if (!string.IsNullOrEmpty(booking.TourId))
                {

                    var tour = await _tourServices.GetByIDAsync(booking.TourId);
                    if(tour != null)
                    {
                        detail.TourTitle = tour.Title.Tr;

                        var tourDate = tour.TourDates.FirstOrDefault(x => x.Id == booking.TourDateId);
                        if(tourDate != null)
                        {
                            detail.StartDate = tourDate.StartDate;
                            detail.EndDate = tourDate.EndDate;
                        }
                    }
                }
            }
            return detail;
        }


        public async Task SetStatusAsync(string id, string status)
        {
            var booking = await _bookingCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (booking is null) return;

            // Zaten iptal edilmiş bir kaydı ikinci kez iptal edip koltuğu iki kez iade etmeyelim iade oldugunda pozitif iade olur 
            var isBecomingCancelled = status == "İptal Edildi" && booking.Status != "İptal Edildi";

            var update = Builders<Booking>.Update.Set(x => x.Status, status);
            await _bookingCollection.UpdateOneAsync(x => x.Id == id, update);

            if (isBecomingCancelled && !string.IsNullOrEmpty(booking.TourId) && !string.IsNullOrEmpty(booking.TourDateId))
            {
                var seats = booking.AdultCount + booking.ChildCount;
                await _tourServices.UpdateSeatsAsync(booking.TourId, booking.TourDateId, seats); 
            }
        }

        public async Task SetPaymentStatusAsync(string id, string paymentStatus)
        {
            var update = Builders<Booking>.Update.Set(x => x.PaymentStatus, paymentStatus);
            await _bookingCollection.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task<int> GetPendingCountAsync()
        {
            var count = await _bookingCollection.CountDocumentsAsync(x => x.Status == "Bekliyor");
            return (int)count;
        }






        private static string GenerateBookingNumber()
        {
            return $"OH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }

      
        private async Task<List<ResultBookingDto>> EnrichAsync(List<Booking> bookings)
        {
            var result = _mapper.Map<List<ResultBookingDto>>(bookings);

            if (result.Count == 0) return result;

            var tourIds = bookings.Where(x => !string.IsNullOrEmpty(x.TourId)).Select(x => x.TourId!).Distinct().ToList();
            var memberIds = bookings.Select(x => x.MemberId).Distinct().ToList();

            var tours = await _tourServices.GetByIdsAsync(tourIds);
            var members = await _memberServices.GetByIdsAsync(memberIds);

            var tourMap = tours.ToDictionary(x => x.Id!, x => x.Title.Tr ?? "");
            var memberMap = members.ToDictionary(x => x.Id!, x => $"{x.FirstName} {x.LastName}");

            foreach (var dto in result)
            {
                dto.TourTitle = tourMap.GetValueOrDefault(dto.TourId ?? "", "—");
                dto.MemberName = memberMap.GetValueOrDefault(dto.MemberId ?? "", "—");
            }

            return result;
        }
    }
}
