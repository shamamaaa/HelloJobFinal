using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Vacancy
{
    public record CreateVacancyVm(string Position, int Salary, bool HasDriverLicense, DateTime FinishTime, bool IsTimeOver, int ViewCount, int CityId, int EducationId, int ExperienceId, int WorkingHourId, int CategoryId, int CompanyId, string Status);

}

