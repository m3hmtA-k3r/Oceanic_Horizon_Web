using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

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
            await _tourCollection.InsertOneAsync(tour);
        }

        public async Task UpdateAsync(UpdateTourDto updateTourDto)
        {
            var tour = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x => x.Id == tour.Id, tour);
        }
        public async Task DeleteAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.Id == id);
        }

       
    }
}
