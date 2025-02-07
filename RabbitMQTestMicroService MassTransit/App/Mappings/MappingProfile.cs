using App.DTOs.Categories;
using App.DTOs.Vacancies;
using AutoMapper;
using Domain.DTOs.Cities;
using Domain.DTOs.Countries;
using Domain.DTOs.Vacancies;
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

            // Country Mapping
            CreateMap<Country, CountryDto>()
                .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.Cities)); 
            CreateMap<CreateCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>();

            // City Mapping
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name)); 
            CreateMap<CreateCityDto, City>();
            CreateMap<UpdateCityDto, City>();

        }
    }
}
