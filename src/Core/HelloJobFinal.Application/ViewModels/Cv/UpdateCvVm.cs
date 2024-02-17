
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateCvVm
    {
        public string ImageUrl { get; init; }
        public string CvFile { get; init; }
        public IFormFile? Photo { get; init; }
        public IFormFile? CvUFile { get; init; }

        [Required(ErrorMessage = "Ad vacibdir")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Soyad vacibdir")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Soyad 2 ilə 25 simvol aralığında olmalıdır")]
        public string Surname { get; init; }

        [Required(ErrorMessage = "E-poçt vacibdir")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Yanlış e-poçt formatı")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Doğum tarixi daxil edilməlidir.")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; init; }

        [Required(ErrorMessage = "Telefon nömrəsi daxil edilməlidir.")]
        public string Phone { get; init; }

        [Required(ErrorMessage = "Vəzifə daxil edilməlidir.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Vəzifə 2 ilə 40 simvol aralığında olmalıdır")]
        public string Position { get; init; }

        [Required(ErrorMessage = "Minumum əmək haqqı daxil edilməlidir.")]
        [Range(1, 30001, ErrorMessage = "Minumum əmək haqqı 0dan böyük, 30000dən az olmalıdır.")]
        public int MinSalary { get; init; }

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

        public List<IncludeCityVm>? Cities { get; set; }
        public List<IncludeEducationVm>? Educations { get; set; }
        public List<IncludeExperienceVm>? Experiences { get; set; }
        public List<IncludWorkingHourVm>? WorkingHours { get; set; }
        public List<IncludeCategoryItemVm>? CategoryItems { get; set; }
    }

}

