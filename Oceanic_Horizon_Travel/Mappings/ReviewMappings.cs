using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.ReviewDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class ReviewMappings: Profile
    {
        public ReviewMappings()
        {
            CreateMap<Review, ResultReviewDto>();
        }
    }
}
