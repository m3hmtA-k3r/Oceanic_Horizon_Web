using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.BannerDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class BannerMappings: Profile
    {
        public BannerMappings()
        {
            CreateMap<CreateBannerDto, Banner>();
            CreateMap<UpdateBannerDto, Banner>();
            CreateMap<Banner, ResultBannerDto>().ReverseMap();
            CreateMap<ResultBannerDto, UpdateBannerDto>();
        }
    }
}
