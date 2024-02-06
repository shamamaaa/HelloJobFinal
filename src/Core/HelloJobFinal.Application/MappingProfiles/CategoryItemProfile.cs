using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CategoryItemProfile : Profile
	{
		public CategoryItemProfile()
		{
            CreateMap<CreateCategoryItemVm, CategoryItem>().ReverseMap();
            CreateMap<UpdateCategoryItemVm, CategoryItem>().ReverseMap();
            CreateMap<IncludeCategoryItemVm, CategoryItem>().ReverseMap();
        }
	}
}

