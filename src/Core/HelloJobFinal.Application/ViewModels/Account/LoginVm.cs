
using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record LoginVM
    {
        [Required(ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı daxil edilməlidir.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı 2 ilə 255 simvol aralığında olmalıdır.")]
        [RegularExpression(@"^(?:(?![@._])[a-zA-Z0-9@._-]{3,})$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Yanlış istifadəçi adı və ya E-poçt formatı.")]
        public string UserNameOrEmail { get; init; }

        [Required(ErrorMessage = "Parol daxil edilməlidir.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Parol yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər.")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Parol 8 ilə 25 simvol aralığında olmalıdır.")]
        public string Password { get; init; }

        public bool IsRemembered { get; init; }
    }
}

