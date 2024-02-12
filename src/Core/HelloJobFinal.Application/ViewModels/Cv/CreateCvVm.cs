using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCvVm(string Name, string Surname, string Email, DateTime Birthday, int Phone, IFormFile Photo,
        string Position, int MinSalary, bool HasDriverLicense, DateTime FinishTime, IFormFile CvFile, bool IsTimeOver,
        int ViewCount, int CityId, int EducationId, int ExperienceId, int WorkingHourId, int CategoryId, string AppUserId,
        string Status)
    {

        public List<IncludeCityVm> Cities { get; set; }
        public List<IncludeEducationVm> Educations { get; set; }
        public List<IncludeExperienceVm> Experiences { get; set; }
        public List<IncludWorkingHourVm> WorkingHours { get; set; }
        public List<IncludeCategoryItemVm> CategoryItems { get; set; }

    }

}

