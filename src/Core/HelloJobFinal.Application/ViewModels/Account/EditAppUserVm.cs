using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
	public record EditAppUserVm
	{
        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "İstifadəçi adı 2 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "İstifadəçi adı yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string UserName { get; init; }

        [Required(ErrorMessage = "Ad vacibdir")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Ad yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Soyad vacibdir")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Soyad 2 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Soyad yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string Surname { get; init; }
    }
}

