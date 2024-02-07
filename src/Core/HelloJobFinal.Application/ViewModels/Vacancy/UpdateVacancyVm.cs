using System;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;

namespace HelloJobFinal.Application.ViewModels.Vacancy
{
    public record UpdateVacancyVm(string Position, int Salary, bool HasDriverLicense, DateTime FinishTime, bool IsTimeOver,
        int ViewCount, int CityId, int EducationId, int ExperienceId, int WorkingHourId, int CategoryId, int CompanyId,
        string Status)
    {
        public List<IncludeCityVm> Cities { get; set; }
        public List<IncludeEducationVm> Educations { get; set; }
        public List<IncludeExperienceVm> Experiences { get; set; }
        public List<IncludWorkingHourVm> WorkingHours { get; set; }
        public List<IncludeCategoryItemVm> CategoryItems { get; set; }
    }

}

