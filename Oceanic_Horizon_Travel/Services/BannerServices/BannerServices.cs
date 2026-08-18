using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Oceanic_Horizon_Travel.DTOs.BannerDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.BannerServices
{
    public class BannerServices : IBannerServices
    {
        private readonly IMongoCollection<Banner> _bannerCollection;
        private readonly IMapper _mapper;


        public BannerServices(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString); //MongoDB ile konuşabilecek bir Client oluşturdum
            var database = client.GetDatabase(databaseSettings.DatabaseName); //hangi database ile calışacağımı seçiyorum

            _bannerCollection = database.GetCollection<Banner>(databaseSettings.BannerCollectionName); //burda yaptıgım işlem toplamı
            //Şu oluyor=> MongoDB ile konusacak müşteriyi oluştur sonra hangi DB adı ile çalışacaksan onu seç sonra seçtigin dbnin tablosuna bunları yaz.
        }


        public async Task<List<ResultBannerDto>> GetAllAsync()
        {
            var banners = await _bannerCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultBannerDto>>(banners);
        }

        public async Task<ResultBannerDto> GetByIdAsync(string id)
        {
            var banners = await _bannerCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<ResultBannerDto>(banners);

        }

        public async Task CreateAsync(CreateBannerDto createBannerDto)
        {
            var banner = _mapper.Map<Banner>(createBannerDto);
            banner.CreatedDate = DateTime.UtcNow;

            await _bannerCollection.InsertOneAsync(banner);
        }

        public async Task UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            var banner = _mapper.Map<Banner>(updateBannerDto);

            var existing = await _bannerCollection.Find(x => x.Id == banner.Id).FirstOrDefaultAsync();
            if (existing is not null)
                banner.CreatedDate = existing.CreatedDate;

            await _bannerCollection.FindOneAndReplaceAsync(x => x.Id == banner.Id, banner);
        }


        public async Task DeleteAsync(string id)
        {
            await _bannerCollection.DeleteOneAsync(x => x.Id == id);
        }

      

       
    }
}
