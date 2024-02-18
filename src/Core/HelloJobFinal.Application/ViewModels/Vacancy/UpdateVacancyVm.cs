using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateVacancyVm
    {
        [Required(ErrorMessage = "Vəzifə daxil edilməlidir.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Vəzifə 2 ilə 40 simvol aralığında olmalıdır")]
        public string Position { get; init; }

        [Required(ErrorMessage = "Əmək haqqı daxil edilməlidir.")]
        [Range(1, 30001, ErrorMessage = "Əmək haqqı 0dan böyük, 30000dən az olmalıdır.")]
        public int Salary { get; init; }

        public bool HasDriverLicense { get; init; }


        [Required(ErrorMessage = "Şəhər daxil edilməlidir.")]
        public int CityId { get; init; }
        [Required(ErrorMessage = "Təhsil daxil edilməlidir.")]
        public int EducationId { get; init; }
        [Required(ErrorMessage = "Staj daxil edilməlidir.")]
        public int ExperienceId { get; init; }
        [Required(ErrorMessage = "İş saatı daxil edilməlidir.")]
        public int WorkingHourId { get; init; }
        [Required(ErrorMessage = "Kateqoriya daxil edilməlidir.")]
        public int CategoryItemId { get; init; }
        [Required(ErrorMessage = "Şirkət daxil edilməlidir.")]
        public int CompanyId { get; init; }

        public List<IncludeCityVm>? Cities { get; set; }
        public List<IncludeEducationVm>? Educations { get; set; }
        public List<IncludeExperienceVm>? Experiences { get; set; }
        public List<IncludWorkingHourVm>? WorkingHours { get; set; }
        public List<IncludeCategoryItemVm>? CategoryItems { get; set; }
        public List<IncludeCompanyVm>? Companies { get; set; }

        public List<IncludeWorkInfo>? WorkInfos { get; set; }
        public List<IncludeRequirement>? Requirements { get; set; }


        public string? WorkInfo { get; init; }
        public List<int>? DeleteWork { get; set; }

        public string? EmployeeRequirement { get; init; }
        public List<int>? DeleteEmployeers { get; set; }
    }

}

