using AutoMapper;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.SiteSettingsServices
{
    public class SiteSettingsServices : ISiteSettingsServices
    {
        private readonly IMongoCollection<SiteSettings> _siteSettingsCollection;
        private readonly IMapper _mapper;

        public SiteSettingsServices(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _siteSettingsCollection = database.GetCollection<SiteSettings>(databaseSettings.SiteSettingsCollectionName);
        }

        public async Task<ResultSiteSettingsDto?> GetAsync()
        {
            // Filtre yok
            var settings = await _siteSettingsCollection.Find(_ => true).FirstOrDefaultAsync();

            return settings is null ? null : _mapper.Map<ResultSiteSettingsDto>(settings);
        }

        public async Task SaveAsync(UpdateSiteSettingsDto updateSiteSettingsDto)
        {
            var existing = await _siteSettingsCollection.Find(_ => true).FirstOrDefaultAsync();
            var settings = _mapper.Map<SiteSettings>(updateSiteSettingsDto);

            if (existing is null)
            {
                // İlk kayıt
                settings.CreatedDate = DateTime.UtcNow;
                await _siteSettingsCollection.InsertOneAsync(settings);
            }
            else
            {

                settings.Id = existing.Id;
                settings.CreatedDate = existing.CreatedDate;

                await _siteSettingsCollection.FindOneAndReplaceAsync(x => x.Id == existing.Id, settings);
            }
        }
    }
}
