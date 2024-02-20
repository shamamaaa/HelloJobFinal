using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CityProfile : Profile
	{
		public CityProfile()
		{
            CreateMap<CreateCityVm, City>().ReverseMap();
            CreateMap<UpdateCityVm, City>().ReverseMap();
            CreateMap<IncludeCityVm, City>().ReverseMap();
            CreateMap<ItemCityVm, City>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()))
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()));
            CreateMap<GetCityVm, City>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()))
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()));


        }
    }
}

