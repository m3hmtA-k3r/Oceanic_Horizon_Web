using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.CategoryDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class CategoryMappings: Profile
    {
        public CategoryMappings()
        {
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, ResultCategoryDto>();
            CreateMap<ResultCategoryDto, UpdateCategoryDto>();
        }
    }
}
