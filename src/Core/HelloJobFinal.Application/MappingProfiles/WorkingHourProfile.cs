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
            CreateMap<ItemWorkingHourVm, WorkingHour>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<GetWorkingHourVm, WorkingHour>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));


        }
    }
}

