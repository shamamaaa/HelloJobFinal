using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class EducationProfile : Profile
	{
		public EducationProfile()
		{
            CreateMap<CreateEducationVm, Education>().ReverseMap();
            CreateMap<UpdateEducationVm, Education>().ReverseMap();
            CreateMap<IncludeEducationVm, Education>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<GetEducationVm, Education>().ReverseMap()
                .ForMember(x => x.IncludeVacancyVms, opt => opt.MapFrom(src => src.Vacancies.ToList()))
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<ItemEducationVm, Education>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));

        }
    }
}

