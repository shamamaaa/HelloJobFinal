using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class WorkInfoProfile : Profile
    {
        public WorkInfoProfile()
        {
            CreateMap<IncludeWorkInfo, WorkInfo>().ReverseMap();
        }
    }
}

