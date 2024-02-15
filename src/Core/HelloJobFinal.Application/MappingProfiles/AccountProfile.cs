using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AppUser, LoginVM>().ReverseMap();
            CreateMap<AppUser, RegisterVM>().ReverseMap();
            CreateMap<FindAccountVm, AppUser>().ReverseMap();
            CreateMap<EditAppUserVm, AppUser>().ReverseMap();
            CreateMap<ItemAppUserVm, AppUser>().ReverseMap();
            CreateMap<GetAppUserVM, AppUser>().ReverseMap()
                .ForMember(x => x.Companies, opt => opt.MapFrom(src => src.Companies.ToList()))
                .ForMember(x => x.Cvs, opt => opt.MapFrom(src => src.Cvs.ToList()))
                .ForMember(x => x.WishListCvs, opt => opt.MapFrom(src => src.WishListCvs.Select(a => a.Cv).ToList()))
                .ForMember(x => x.WishListVacancies, opt => opt.MapFrom(src => src.WishListVacancies.Select(a => a.Vacancy).ToList()))
                .ForMember(x => x.CvRequests, opt => opt.MapFrom(src => src.CvRequests.Select(a => a.Cv).ToList()))
                .ForMember(x => x.VacancyRequests, opt => opt.MapFrom(src => src.VacancyRequests.Select(a => a.Vacancy).ToList()));


        }
    }
}

