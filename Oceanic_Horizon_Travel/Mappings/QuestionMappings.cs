using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.QuestionDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class QuestionMappings: Profile
    {
        public QuestionMappings()
        {
            CreateMap<Question, ResultQuestionDto>();
        }
    }
}
