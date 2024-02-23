using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
	internal class BaseCategoryProfile : Profile
	{
		public BaseCategoryProfile()
		{
            CreateMap<CreateBaseCategoryVm, BaseCategory>().ReverseMap();
            CreateMap<UpdateBaseCategoryVm, BaseCategory>().ReverseMap();
            CreateMap<IncludeBaseCategoryVm, BaseCategory>().ReverseMap();
            CreateMap<ItemBaseCategoryVm, BaseCategory>().ReverseMap()
                .ForMember(x => x.CategoryItems, opt => opt.MapFrom(src => src.CategoryItems.ToList()));
            CreateMap<GetBaseCategoryVm, BaseCategory>().ReverseMap()
                .ForMember(x => x.CategoryItems, opt => opt.MapFrom(src => src.CategoryItems.ToList()));

        }
    }
}

