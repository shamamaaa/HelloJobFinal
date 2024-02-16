using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateBaseCategoryVm
    {
        [Required(ErrorMessage = "Kateqoriya adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public IFormFile? Photo { get; init; }
    }

}

