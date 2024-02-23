using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
	internal class WorkingHourProfile : Profile
	{
		public WorkingHourProfile()
		{
            CreateMap<CreateWorkingHourVm, WorkingHour>().ReverseMap();
            CreateMap<UpdateWorkingHourVm, WorkingHour>().ReverseMap();
            CreateMap<IncludWorkingHourVm, WorkingHour>().ReverseMap()
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<ItemWorkingHourVm, WorkingHour>().ReverseMap()
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));
            CreateMap<GetWorkingHourVm, WorkingHour>().ReverseMap()
                .ForMember(x => x.IncludeVacancyVms, opt => opt.MapFrom(src => src.Vacancies.ToList()))
                .ForMember(x => x.IncludeCvVms, opt => opt.MapFrom(src => src.Cvs.ToList()));


        }
    }
}

