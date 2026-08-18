using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Oceanic_Horizon_Travel.DTOs.CategoryDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;           

        public CategoryServices(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var categories = await _categoryCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(categories);
        }

        public async Task<ResultCategoryDto> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultCategoryDto>(category);
        }
        public async Task CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            category.CreatedDate = DateTime.UtcNow;

            await _categoryCollection.InsertOneAsync(category);
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
           var category = _mapper.Map<Category>(updateCategoryDto);

            var existing = await _categoryCollection.Find(x => x.Id == category.Id).FirstOrDefaultAsync();
            if (existing is not null)
                category.CreatedDate = existing.CreatedDate;

            await _categoryCollection.FindOneAndReplaceAsync(z => z.Id == category.Id, category);
        }

        public async Task DeleteAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(c => c.Id == id);
        }
    }
}
