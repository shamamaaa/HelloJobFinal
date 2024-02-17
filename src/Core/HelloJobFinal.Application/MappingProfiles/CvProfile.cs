using AutoMapper;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Application.ViewModels;


namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CvProfile : Profile
	{
		public CvProfile()
		{
            CreateMap<CreateCvVm, Cv>().ReverseMap();
            CreateMap<UpdateCvVm, Cv>().ReverseMap();
            CreateMap<IncludeCvVm, Cv>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
            CreateMap<ItemCvVm, Cv>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeAppUser, opt => opt.MapFrom(src => src.AppUser))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
            CreateMap<GetCvVm, Cv>().ReverseMap()
                .ForMember(x => x.IncludeCategoryItem, opt => opt.MapFrom(src => src.CategoryItem))
                .ForMember(x => x.IncludeCity, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.IncludeExperience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(x => x.IncludeWorkingHour, opt => opt.MapFrom(src => src.WorkingHour))
                .ForMember(x => x.IncludeAppUser, opt => opt.MapFrom(src => src.AppUser))
                .ForMember(x => x.IncludeEducation, opt => opt.MapFrom(src => src.Education));
        }
    }
}

