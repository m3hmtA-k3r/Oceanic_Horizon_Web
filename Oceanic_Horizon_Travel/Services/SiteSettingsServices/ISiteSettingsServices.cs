using Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos;

namespace Oceanic_Horizon_Travel.Services.SiteSettingsServices
{
    public interface ISiteSettingsServices
    {
        // Tek kaydı getirir. Hiç kayıt yoksa null döner.
        Task<ResultSiteSettingsDto?> GetAsync();

        // Kayıt yoksa oluşturur, varsa günceller (upsert).
        Task SaveAsync(UpdateSiteSettingsDto updateSiteSettingsDto);
    }
}
