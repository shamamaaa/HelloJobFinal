using System;
using HelloJobFinal.Application.ViewModels.Vacancy;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Company
{
    public record GetCompanyVm(int Id, string ImageUrl, string Email, string AppUserId, string Status, string Name)
    {
        public ICollection<IncludeVacancyVm> Vacancies { get; init; }
    }
}

