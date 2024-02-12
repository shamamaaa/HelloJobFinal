namespace HelloJobFinal.Application.ViewModels
{
    public record GetVacancyVm(int Id, string Position, int Salary, bool HasDriverLicense, DateTime FinishTime, bool IsTimeOver, int ViewCount,
        IncludeCityVm IncludeCity, int CityId, IncludeEducationVm IncludeEducation, int EducationId, IncludeExperienceVm IncludeExperience,
        int ExperienceId, IncludWorkingHourVm IncludeWorkingHour, int WorkingHourId, IncludeCategoryItemVm IncludeCategoryItem, string AppUserId, IncludeAppUserVm IncludeAppUser,
        int CategoryId, IncludeCompanyVm IncludeCompany, int CompanyId, string Status);
}