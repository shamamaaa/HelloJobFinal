﻿using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class EducationProfile : Profile
	{
		public EducationProfile()
		{
            CreateMap<CreateEducationVm, Education>().ReverseMap();
            CreateMap<UpdateEducationVm, Education>().ReverseMap();
            CreateMap<IncludeEducationVm, Education>().ReverseMap();
            CreateMap<GetEducationVm, Education>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<ItemEducationVm, Education>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));

        }
    }
}

