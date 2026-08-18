using AutoMapper;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.ReviewDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.ReviewServices
{
    public class ReviewServices : IReviewServices
    {
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly ITourServices _tourServices;
        private readonly IMemberServices _memberServices;
        private readonly IMapper _mapper;

        public ReviewServices(
            IDatabaseSettings databaseSettings,
            ITourServices tourServices,
            IMemberServices memberServices,
            IMapper mapper)
        {
            _tourServices = tourServices;
            _memberServices = memberServices;
            _mapper = mapper;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reviewCollection = database.GetCollection<Review>(databaseSettings.ReviewCollectionName);
        }

        // ─────────────── VİTRİN ───────────────

        public async Task CreateAsync(CreateReviewDto createReviewDto)
        {
            var review = new Review
            {
                MemberId = createReviewDto.MemberId!,
                Type = createReviewDto.Type ?? "Tour",
                EntityId = createReviewDto.EntityId!,
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment!,
                IsApproved = false,       
                CreatedDate = DateTime.UtcNow
            };

            await _reviewCollection.InsertOneAsync(review);
        }

        public async Task<List<ResultReviewDto>> GetApprovedByTourAsync(string tourId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.EntityId == tourId && x.Type == "Tour" && x.IsApproved)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return await EnrichAsync(reviews);
        }


        public async Task<Dictionary<int, int>> GetRatingDistributionAsync(string tourId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.EntityId == tourId && x.Type == "Tour" && x.IsApproved)
                .ToListAsync();

            var distribution = new Dictionary<int, int>();

            for (int star = 5; star >= 1; star--)
            {
                distribution[star] = reviews.Count(x => x.Rating == star);
            }

            return distribution;
        }







        // ─────────────── ADMİN ───────────────

        public async Task<List<ResultReviewDto>> GetAllAsync(string? status = null)
        {
            var filter = status switch
            {
                "pending" => Builders<Review>.Filter.Where(x => !x.IsApproved),
                "approved" => Builders<Review>.Filter.Where(x => x.IsApproved),
                _ => Builders<Review>.Filter.Empty
            };

            var reviews = await _reviewCollection.Find(filter).ToListAsync();

            // Onay bekleyenler üstte
            var sorted = reviews
                .OrderBy(x => x.IsApproved)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();

            return await EnrichAsync(sorted);
        }

        public async Task<int> GetPendingCountAsync()
        {
            var count = await _reviewCollection.CountDocumentsAsync(x => !x.IsApproved);
            return (int)count;
        }

        public async Task SetApprovalAsync(string id, bool isApproved)
        {

            var review = await _reviewCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            Console.WriteLine($"[1] SetApproval çağrıldı → id={id}, isApproved={isApproved}, review={(review is null ? "NULL" : review.EntityId)}");


            if (review is null) return;

            var update = Builders<Review>.Update.Set(x => x.IsApproved, isApproved);
            await _reviewCollection.UpdateOneAsync(x => x.Id == id, update);


            await RecalculateTourRatingAsync(review.EntityId);
        }

        public async Task DeleteAsync(string id)
        {
            var review = await _reviewCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (review is null) return;

            await _reviewCollection.DeleteOneAsync(x => x.Id == id);

            await RecalculateTourRatingAsync(review.EntityId);
        }









        //             YARDIMCI  

        private async Task RecalculateTourRatingAsync(string tourId)
        {
            var approved = await _reviewCollection
                .Find(x => x.EntityId == tourId && x.Type == "Tour" && x.IsApproved)
                .ToListAsync();

            var count = approved.Count;
            var average = count > 0 ? Math.Round(approved.Average(x => x.Rating), 1) : 0;

            Console.WriteLine($"[2] Recalculate → tourId={tourId}, onaylıSayı={count}, ortalama={average}");

            await _tourServices.UpdateRatingAsync(tourId, average, count);
        }

        // Tur adı ve üye adını doldurur — N+1 önlemi için toplu çekiyor
        private async Task<List<ResultReviewDto>> EnrichAsync(List<Review> reviews)
        {
            var result = _mapper.Map<List<ResultReviewDto>>(reviews);

            if (result.Count == 0) return result;

            var tourIds = reviews.Where(x => x.Type == "Tour").Select(x => x.EntityId).Distinct().ToList();
            var memberIds = reviews.Select(x => x.MemberId).Distinct().ToList();

            var tours = await _tourServices.GetByIdsAsync(tourIds);
            var members = await _memberServices.GetByIdsAsync(memberIds);

            var tourMap = tours.ToDictionary(x => x.Id!, x => x.Title.Tr ?? "");
            var memberMap = members.ToDictionary(x => x.Id!, x => $"{x.FirstName} {x.LastName}");

            foreach (var dto in result)
            {
                dto.EntityTitle = tourMap.GetValueOrDefault(dto.EntityId!, "—");
                dto.MemberName = memberMap.GetValueOrDefault(dto.MemberId!, "—");
            }

            return result;
        }
    }
}
