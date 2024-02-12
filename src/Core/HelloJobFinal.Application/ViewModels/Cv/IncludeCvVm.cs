

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeCvVm(int Id, string Name, string Surname,
        string Email, DateTime Birthday, int Phone, string ImageUrl,
        string Position, int MinSalary, bool HasDriverLicense, DateTime FinishTime,
        string CvUrl, bool IsTimeOver, int ViewCount, IncludeCityVm IncludeCity, int CityId,
        IncludeEducationVm IncludeEducation, int EducationId, IncludeExperienceVm IncludeExperience, int ExperienceId,
        IncludWorkingHourVm IncludeWorkingHour, int WorkingHourId, IncludeCategoryItemVm IncludeCategoryItem, int CategoryId,
        IncludeAppUserVm IncludeAppUser, string AppUserId, string Status);

}

