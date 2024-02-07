using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CityProfile : Profile
	{
		public CityProfile()
		{
            CreateMap<CreateCityVm, City>().ReverseMap();
            CreateMap<UpdateCityVm, City>().ReverseMap();
            CreateMap<IncludeEducation, City>().ReverseMap();
            CreateMap<ItemCityVm, City>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Vacancies.ToList()));
            CreateMap<GetCityVm, City>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Vacancies.ToList()));



        }
    }
}

