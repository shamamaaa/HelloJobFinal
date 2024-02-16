using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
	public record FindAccountVm
	{
        [Required(ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı daxil edilməlidir.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "İstifadəçi adı və ya E-poçt ünvanı 2 ilə 255 simvol aralığında olmalıdır.")]
        [RegularExpression(@"^(?:(?![@._])[a-zA-Z0-9@._-]{3,})$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Yanlış istifadəçi adı və ya E-poçt formatı.")]
        public string UserNameOrEmail { get; init; }
    }
}

