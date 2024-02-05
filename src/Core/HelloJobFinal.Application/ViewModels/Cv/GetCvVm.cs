using HelloJobFinal.Application.ViewModels.Account;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels.Cv
{
    public record GetCvVm(int Id, string Name, string Surname, string Email, DateTime Birthday,
        int Phone, string ImageUrl, string Position, int MinSalary, bool HasDriverLicense,
        DateTime FinishTime, string CvUrl, bool IsTimeOver, int ViewCount, IncludeCityVm IncludeCity,
        int CityId, IncludeEducationVm IncludeEducation, int EducationId, IncludeExperienceVm IncludeExperience,
        int ExperienceId, IncludWorkingHourVm IncludeWorkingHour, int WorkingHourId, IncludeCategoryItemVm IncludeCategoryItem,
        int CategoryId, IncludeAppUserVm IncludeAppUser, string AppUserId, string Status);

}

