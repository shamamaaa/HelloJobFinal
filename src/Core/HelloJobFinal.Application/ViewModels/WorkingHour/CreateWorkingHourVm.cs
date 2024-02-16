using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateWorkingHourVm
    {
        [Required(ErrorMessage = "İş-saatı adı daxil edilməlidir.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 40 simvol aralığında olmalıdır")]
        public string Name { get; init; }
    }

}

