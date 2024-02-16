using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateCityVm
    {
        [Required(ErrorMessage = "Şəhər adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
        public string Name { get; init; }
    }
}

