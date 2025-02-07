using App.DTOs.Categories;
using AutoMapper;
using Domain.Entites;
using Domain.Entities;


namespace App.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mapping
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

        }
    }
}
