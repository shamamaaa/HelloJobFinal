using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCvVm
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
        public DateTime Birthday { get; init; }
        public int Phone { get; init; }
        public IFormFile Photo { get; init; }
        public string Position { get; init; }
        public int MinSalary { get; init; }
        public bool HasDriverLicense { get; init; }
        public DateTime FinishTime { get; init; }
        public IFormFile CvFile { get; init; }
        public bool IsTimeOver { get; init; }
        public int ViewCount { get; init; }
        public int CityId { get; init; }
        public int EducationId { get; init; }
        public int ExperienceId { get; init; }
        public int WorkingHourId { get; init; }
        public int CategoryId { get; init; }
        public string AppUserId { get; init; }
        public string Status { get; init; }

        public List<IncludeCityVm> Cities { get; set; }
        public List<IncludeEducationVm> Educations { get; set; }
        public List<IncludeExperienceVm> Experiences { get; set; }
        public List<IncludWorkingHourVm> WorkingHours { get; set; }
        public List<IncludeCategoryItemVm> CategoryItems { get; set; }
    }
}
