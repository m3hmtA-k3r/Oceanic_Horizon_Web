using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class SiteSettingsMappings : Profile
    {
        public SiteSettingsMappings()
        {
            CreateMap<UpdateSiteSettingsDto, SiteSettings>();
            CreateMap<SiteSettings, ResultSiteSettingsDto>();
            CreateMap<ResultSiteSettingsDto, UpdateSiteSettingsDto>();
        }
    }
}
