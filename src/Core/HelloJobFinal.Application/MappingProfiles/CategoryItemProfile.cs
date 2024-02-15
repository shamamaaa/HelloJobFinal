using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CategoryItemProfile : Profile
	{
		public CategoryItemProfile()
		{
            CreateMap<CreateCategoryItemVm, CategoryItem>().ReverseMap();
            CreateMap<UpdateCategoryItemVm, CategoryItem>().ReverseMap();
            CreateMap<IncludeCategoryItemVm, CategoryItem>().ReverseMap()
                .ForMember(x => x.IncludeBaseCategory, opt => opt.MapFrom(src => src.BaseCategory))
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()))
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<ItemCategoryItemVm, CategoryItem>().ReverseMap()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()))
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<GetCategoryItemVm, CategoryItem>().ReverseMap()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()))
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()));
        }
    }
}

