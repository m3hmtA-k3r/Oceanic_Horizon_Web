using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Oceanic_Horizon_Travel.DTOs.DestinationDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.DestinationServices
{
    public class DestinationServices : IDestinationServices
    {
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMapper _mapper;

        public DestinationServices(IDatabaseSettings databaseSettings,IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
        }



        public async Task<List<ResultDestinationDto>> GetAllAsync()
        {
            var destinations = await _destinationCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultDestinationDto>>(destinations);
        }

        public async Task<ResultDestinationDto> GetByIdAsync(string id)
        {
            var destinations = await _destinationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultDestinationDto>(destinations);
        }

        public async Task CreateAsync(CreateDestinationDto createDestinationDto)
        {
            var destination = _mapper.Map<Destination>(createDestinationDto);
            await _destinationCollection.InsertOneAsync(destination);
        }       

        public async Task UpdateAsync(UpdateDestinationDto updateDestinationDto)
        {
            var destination = _mapper.Map<Destination>(updateDestinationDto);
            await _destinationCollection.FindOneAndReplaceAsync(x => x.Id == destination.Id, destination);
        }
        public async Task DeleteAsync(string id)
        {
            await _destinationCollection.DeleteOneAsync(z => z.Id == id);
        }
    }
}
