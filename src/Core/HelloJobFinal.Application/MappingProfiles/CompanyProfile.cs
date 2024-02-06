using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class CompanyProfile : Profile
	{
		public CompanyProfile()
		{
            CreateMap<CreateCompanyVm, Company>().ReverseMap();
            CreateMap<UpdateCompanyVm, Company>().ReverseMap();
            CreateMap<IncludeCompanyVm, Company>().ReverseMap();
        }
	}
}

