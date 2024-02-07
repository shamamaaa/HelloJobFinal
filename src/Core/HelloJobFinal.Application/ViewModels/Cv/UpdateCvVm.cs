using System;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Cv
{
    public record UpdateCvVm(string Name, string Surname, string Email, DateTime Birthday, int Phone,
        string ImageUrl, IFormFile? Photo, string Position, int MinSalary, bool HasDriverLicense, DateTime FinishTime,
        string CvUrl, IFormFile? CvFile, bool IsTimeOver, int ViewCount, int CityId, int EducationId, int ExperienceId,
        int WorkingHourId, int CategoryId, string AppUserId, string Status)
    {
        public List<IncludeCityVm> Cities { get; set; }
        public List<IncludeEducationVm> Educations { get; set; }
        public List<IncludeExperienceVm> Experiences { get; set; }
        public List<IncludWorkingHourVm> WorkingHours { get; set; }
        public List<IncludeCategoryItemVm> CategoryItems { get; set; }
    }

}

