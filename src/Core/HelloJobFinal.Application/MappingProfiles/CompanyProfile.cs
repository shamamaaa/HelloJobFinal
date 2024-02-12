using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CompanyProfile : Profile
	{
		public CompanyProfile()
		{
            CreateMap<CreateCompanyVm, Company>().ReverseMap();
            CreateMap<UpdateCompanyVm, Company>().ReverseMap();
            CreateMap<IncludeCompanyVm, Company>().ReverseMap()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()));
            CreateMap<ItemCompanyVm, Company>().ReverseMap()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()));
            CreateMap<GetCompanyVm, Company>().ReverseMap()
                .ForMember(x => x.Vacancies, opt => opt.MapFrom(src => src.Vacancies.ToList()));

        }
    }
}

