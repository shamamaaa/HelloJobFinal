using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.WorkingHour;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
	internal class WorkingHourProfile : Profile
	{
		public WorkingHourProfile()
		{
            CreateMap<CreateWorkingHourVm, WorkingHour>().ReverseMap();
            CreateMap<UpdateWorkingHourVm, WorkingHour>().ReverseMap();
            CreateMap<IncludWorkingHourVm, WorkingHour>().ReverseMap();
        }
	}
}

