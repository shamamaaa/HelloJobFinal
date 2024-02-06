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
            CreateMap<IncludeCityVm, City>().ReverseMap();
        }
	}
}

