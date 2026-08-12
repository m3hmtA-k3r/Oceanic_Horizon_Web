using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class MemberMappings: Profile
    {
        public MemberMappings()
        {
            CreateMap<RegisterMemberDto, Member>();
        }
    }
}
