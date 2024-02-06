using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Vacancy;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class VacancyProfile : Profile
	{
		public VacancyProfile()
		{
            CreateMap<CreateVacancyVm, Vacancy>().ReverseMap();
            CreateMap<UpdateVacancyVm, Vacancy>().ReverseMap();
            CreateMap<IncludeVacancyVm, Vacancy>().ReverseMap();
        }
	}
}

