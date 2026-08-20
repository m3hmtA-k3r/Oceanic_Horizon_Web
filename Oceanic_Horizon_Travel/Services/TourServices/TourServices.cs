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
            if(ids is null || ids.Count == 0)            
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
    }
}
