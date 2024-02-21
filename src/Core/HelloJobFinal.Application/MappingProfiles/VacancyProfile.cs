using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class VacancyProfile : Profile
	{
		public VacancyProfile()
		{
            CreateMap<CreateVacancyVm, Vacancy>().ReverseMap();
            CreateMap<UpdateVacancyVm, Vacancy>().ReverseMap()
                .ForMember(x => x.WorkInfos, opt => opt.MapFrom(src => src.WorkInfos.ToList()))
                .ForMember(x => x.Requirements, opt => opt.MapFrom(src => src.Requirements.ToList()));
            CreateMap<IncludeVacancyVm, Vacancy>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeCompany, opt => opt.MapFrom(src => src.Company))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
            CreateMap<ItemVacancyVm, Vacancy>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeCompany, opt => opt.MapFrom(src => src.Company))
                .ForMember(x => x.IncludeAppUser, opt => opt.MapFrom(src => src.AppUser))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
            CreateMap<GetVacancyVm, Vacancy>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.AllWorkInfos, opt => opt.MapFrom(src => src.WorkInfos.ToList()))
                .ForMember(x => x.AllEmployeerInfos, opt => opt.MapFrom(src => src.Requirements.ToList()))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeCompany, opt => opt.MapFrom(src => src.Company))
                .ForMember(x => x.IncludeAppUser, opt => opt.MapFrom(src => src.AppUser))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
        }
    }
}

