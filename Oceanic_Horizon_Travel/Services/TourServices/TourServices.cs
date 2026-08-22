using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;
using System.Text.RegularExpressions;

namespace Oceanic_Horizon_Travel.Services.TourServices
{
    public class TourServices : ITourServices
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;

        public TourServices(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
        }

        public async Task<List<ResultTourDto>> GetAllAsync()
        {
            var tours = await _tourCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(tours);
        }

        public async Task<ResultTourDto> GetByIDAsync(string id)
        {
            var tours = await _tourCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultTourDto>(tours);
        }

        public async Task<List<ResultTourDto>> GetByIdsAsync(List<string> ids) // Toplu çekme metotları 
        {
            if (ids is null || ids.Count == 0)
                return new List<ResultTourDto>();

            var tours = await _tourCollection.Find(x => ids.Contains(x.Id)).ToListAsync();

            return _mapper.Map<List<ResultTourDto>>(tours);
        }

        public async Task CreateAsync(CreateTourDto createTourDto)
        {
            var tour = _mapper.Map<Tour>(createTourDto);
            tour.CreatedDate = DateTime.UtcNow;

            await _tourCollection.InsertOneAsync(tour);
        }

        public async Task UpdateAsync(UpdateTourDto updateTourDto)
        {
            var tour = _mapper.Map<Tour>(updateTourDto);

            var existing = await _tourCollection.Find(x => x.Id == tour.Id).FirstOrDefaultAsync();
            if (existing is not null)
                tour.CreatedDate = existing.CreatedDate;

            await _tourCollection.FindOneAndReplaceAsync(x => x.Id == tour.Id, tour);
        }
        public async Task DeleteAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task UpdateRatingAsync(string tourId, double rating, int reviewCount)
        {
            var update = Builders<Tour>.Update
                .Set(x => x.Rating, rating)
                .Set(x => x.ReviewCount, reviewCount);

            var result = await _tourCollection.UpdateOneAsync(x => x.Id == tourId, update);

            Console.WriteLine($"[3] UpdateRating → tourId={tourId}, rating={rating}, count={reviewCount}, Matched={result.MatchedCount}, Modified={result.ModifiedCount}");
        }

        public async Task UpdateSeatsAsync(string tourId, string tourDateId, int delta)
        {

            var filter = Builders<Tour>.Filter.And(// Önce turu, sonra o turun icindeki doğru kalkıs tarihini bul
                Builders<Tour>.Filter.Eq(x => x.Id, tourId),
                Builders<Tour>.Filter.ElemMatch(x => x.TourDates, d => d.Id == tourDateId)
            );

            var update = Builders<Tour>.Update.Inc(x => x.TourDates.FirstMatchingElement().AvailableSeats, delta);


            await _tourCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<ResultTourDto>> SearchAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return new List<ResultTourDto>();

            // Kullanıcının yazdığı metni düz metin olarak ara
            var pattern = new BsonRegularExpression(Regex.Escape(term), "i");

            var filter = Builders<Tour>.Filter.Or(
                Builders<Tour>.Filter.Regex(x => x.Title.Tr, pattern),
                Builders<Tour>.Filter.Regex(x => x.Title.En, pattern),
                Builders<Tour>.Filter.Regex(x => x.Title.Pt, pattern),
                Builders<Tour>.Filter.Regex(x => x.SeoUrl, pattern)
            );

            var tours = await _tourCollection.Find(filter).Limit(10).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(tours);
        }

        public async Task<(List<ResultTourDto> Items, int TotalCount)> GetFilteredAsync(TourFilterDto filter)
        {
            var builder = Builders<Tour>.Filter;

            // Vitrinde sadece aktif turlar
            var conditions = new List<FilterDefinition<Tour>>
    {
        builder.Eq(x => x.IsActive, true)
    };

            if (!string.IsNullOrWhiteSpace(filter.Q))
            {
                var pattern = new BsonRegularExpression(Regex.Escape(filter.Q), "i");
                conditions.Add(builder.Or(
                    builder.Regex(x => x.Title.Tr, pattern),
                    builder.Regex(x => x.Title.En, pattern),
                    builder.Regex(x => x.Title.Pt, pattern),
                    builder.Regex(x => x.Route.Tr, pattern)
                ));
            }

            if (!string.IsNullOrWhiteSpace(filter.DestinationId))
                conditions.Add(builder.Eq(x => x.DestinationId, filter.DestinationId));

            if (!string.IsNullOrWhiteSpace(filter.CategoryId))
                conditions.Add(builder.Eq(x => x.CategoryId, filter.CategoryId));

            if (filter.MinPrice.HasValue)
                conditions.Add(builder.Gte(x => x.BasePrice, filter.MinPrice.Value));

            if (filter.MaxPrice.HasValue)
                conditions.Add(builder.Lte(x => x.BasePrice, filter.MaxPrice.Value));

            if (filter.MinDay.HasValue)
                conditions.Add(builder.Gte(x => x.Day, filter.MinDay.Value));

            if (filter.MaxDay.HasValue)
                conditions.Add(builder.Lte(x => x.Day, filter.MaxDay.Value));

            var combined = builder.And(conditions);

            var sort = filter.Sort switch
            {
                "price-asc" => Builders<Tour>.Sort.Ascending(x => x.BasePrice),
                "price-desc" => Builders<Tour>.Sort.Descending(x => x.BasePrice),
                "rating" => Builders<Tour>.Sort.Descending(x => x.Rating),
                _ => Builders<Tour>.Sort.Descending(x => x.IsFeatured).Descending(x => x.CreatedDate)
            };

            var pageSize = 9;
            var page = filter.Page < 1 ? 1 : filter.Page;

            var totalCount = (int)await _tourCollection.CountDocumentsAsync(combined);

            var tours = await _tourCollection
                .Find(combined)
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (_mapper.Map<List<ResultTourDto>>(tours), totalCount);
        }

        public async Task<ResultTourDto?> GetBySeoUrlAsync(string seoUrl)
        {
            var tour = await _tourCollection
                .Find(x => x.SeoUrl == seoUrl && x.IsActive)
                .FirstOrDefaultAsync();

            return tour is null ? null : _mapper.Map<ResultTourDto>(tour);
        }

        public async Task<ResultTourDto?> GetByTourDateIdAsync(string tourDateId) //Rezervasyon akışı
        {
            // Kalkış tarihi Tour belgesinin içinde — diziye ElemMatch ile iniyoruz
            var filter = Builders<Tour>.Filter.ElemMatch(x => x.TourDates, d => d.Id == tourDateId);

            var tour = await _tourCollection.Find(filter).FirstOrDefaultAsync();
            return tour is null ? null : _mapper.Map<ResultTourDto>(tour);
        }


    }
}
