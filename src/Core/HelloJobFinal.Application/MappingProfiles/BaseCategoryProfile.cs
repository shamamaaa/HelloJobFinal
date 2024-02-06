using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Category;
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
        }
	}
}

