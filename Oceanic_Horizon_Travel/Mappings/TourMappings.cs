using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class TourMappings: Profile
    {
        public TourMappings()
        {
            CreateMap<CreateTourDto, Tour>();
            CreateMap<UpdateTourDto, Tour>();
            CreateMap<Tour, ResultTourDto>();
            CreateMap<ResultTourDto, UpdateTourDto>();
        }
    }
}
