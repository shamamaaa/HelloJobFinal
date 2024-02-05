using System;
using HelloJobFinal.Application.ViewModels.Account;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;

namespace HelloJobFinal.Application.ViewModels.Vacancy
{
	public record GetVacancyVm(int Id, string Position, int Salary, bool HasDriverLicense, DateTime FinishTime, bool IsTimeOver, int ViewCount,
        IncludeCityVm IncludeCity, int CityId, IncludeEducationVm IncludeEducation, int EducationId, IncludeExperienceVm IncludeExperience,
        int ExperienceId, IncludWorkingHourVm IncludeWorkingHour, int WorkingHourId, IncludeCategoryItemVm IncludeCategoryItem,
        int CategoryId, IncludeCompanyVm IncludeCompany, int CompanyId, string Status);
}