namespace HelloJobFinal.Application.ViewModels
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

