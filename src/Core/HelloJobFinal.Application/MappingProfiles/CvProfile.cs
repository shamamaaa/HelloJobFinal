using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Cv;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CvProfile : Profile
	{
		public CvProfile()
		{
            CreateMap<CreateCvVm, Cv>().ReverseMap();
            CreateMap<UpdateCvVm, Cv>().ReverseMap();
            CreateMap<IncludeCvVm, Cv>().ReverseMap();
        }
	}
}

