namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeVacancyVm
    {
        public int Id { get; init; }
        public string Position { get; init; }
        public int Salary { get; init; }
        public bool HasDriverLicense { get; init; }
        public DateTime FinishTime { get; init; }
        public DateTime CreatedAt { get; init; }

        public bool IsTimeOver { get; init; }
        public int ViewCount { get; init; }
        public IncludeCityVm IncludeCity { get; init; }
        public int CityId { get; init; }
        public IncludeEducationVm IncludeEducation { get; init; }
        public int EducationId { get; init; }
        public IncludeExperienceVm IncludeExperience { get; init; }
        public int ExperienceId { get; init; }
        public IncludWorkingHourVm IncludeWorkingHour { get; init; }
        public int WorkingHourId { get; init; }
        public IncludeCategoryItemVm IncludeCategoryItem { get; init; }
        public string AppUserId { get; init; }
        public IncludeAppUserVm IncludeAppUser { get; init; }
        public int CategoryId { get; init; }
        public IncludeCompanyVm IncludeCompany { get; init; }
        public int CompanyId { get; init; }
        public string Status { get; init; }
    }
	
}

