using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetAppUserVM
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public bool IsActivate { get; set; }

        public List<IncludeCompanyVm>? Companies { get; set; }  //Company daxilinden vakansiyalara el catir
        public List<IncludeCvVm>? Cvs { get; set; }  //Employee elan kimi bir nece cv yerelesdire bilermis.
        public List<IncludeVacancyVm>? Vacancies { get; set; }

        public List<IncludeCvVm>? WishListCvs { get; set; }
        public List<IncludeVacancyVm>? WishListVacancies { get; set; }
        public List<IncludeCvVm>? CvRequests { get; set; }
        public List<IncludeVacancyVm>? VacancyRequests { get; set; }
    }
}

