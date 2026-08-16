using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.DestinationDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class DestinationMappings: Profile
    {
        public DestinationMappings()
        {
            CreateMap<CreateDestinationDto, Destination>();
            CreateMap<UpdateDestinationDto, Destination>();
            CreateMap<Destination, ResultDestinationDto>();
            CreateMap<ResultDestinationDto, UpdateDestinationDto>();
        }
    }
}
