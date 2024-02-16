﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateExperienceVm
    {
        [Required(ErrorMessage = "Staj adı daxil edilməlidir.")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 32 simvol aralığında olmalıdır")]
        public string Name { get; init; }
    }

}

