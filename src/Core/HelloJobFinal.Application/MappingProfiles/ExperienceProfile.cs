using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class ExperienceProfile : Profile
	{
		public ExperienceProfile()
		{
            CreateMap<CreateExperienceVm, Experience>().ReverseMap();
            CreateMap<UpdateExperienceVm, Experience>().ReverseMap();
            CreateMap<IncludeExperienceVm, Experience>().ReverseMap();
        }
	}
}

