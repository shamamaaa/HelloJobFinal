using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateSettingVm
    {
        public string Key { get; set; } = null!;
        [Required(ErrorMessage = "Dəyər daxil edilməlidir")]
        [StringLength(1500, MinimumLength = 1, ErrorMessage = "Dəyər 1 ilə 1500 simvol aralığında olmalıdır")]
        public string Value { get; set; } = null!;
    }
}

