using System;
using System.ComponentModel.DataAnnotations;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateCompanyVm
    {
        public IFormFile? Photo { get; init; }
        public string ImageUrl { get; init; }

        [Required(ErrorMessage = "E-poçt vacibdir")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
        [EmailAddress(ErrorMessage = "Yanlış e-poçt ünvanı")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Yanlış e-poçt formatı")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Ad vacibdir")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 32 simvol aralığında olmalıdır")]
        public string Name { get; init; }
    }

}

