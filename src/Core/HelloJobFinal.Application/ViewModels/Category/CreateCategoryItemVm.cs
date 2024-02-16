using System;
using System.ComponentModel.DataAnnotations;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCategoryItemVm
    {
        [Required(ErrorMessage = "Kateqoriya adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
        public string Name { get; init; }
        [Required(ErrorMessage = "Base-category daxil edilməlidir.")]
        public int BaseCategoryId { get; init; }
        public List<IncludeBaseCategoryVm>? Categorys { get; set; }
    }
}

