using System;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Cv;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;

namespace HelloJobFinal.Application.ViewModels.Vacancy
{
	public class VacancyFilterVM
	{
        public List<ItemVacancyVm> Vacancys { get; set; }
        public List<IncludeCategoryItemVm> Categories { get; set; }
        public List<IncludeCityVm> Cities { get; set; }
        public List<IncludeEducationVm> Educations { get; set; }
        public List<IncludeExperienceVm> Experiences { get; set; }
        public List<IncludWorkingHourVm> WorkingHours { get; set; }
    }
}

