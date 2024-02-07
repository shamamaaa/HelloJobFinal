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
            CreateMap<ItemExperienceVm, Experience>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<GetExperienceVm, Experience>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));

        }
    }
}

