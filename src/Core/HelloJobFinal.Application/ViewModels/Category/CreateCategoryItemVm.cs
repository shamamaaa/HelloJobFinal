using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCategoryItemVm
    {
        public string Name { get; init; }
        public int BaseCategoryId { get; init; }
        public List<IncludeBaseCategoryVm>? Categorys { get; set; }
    }
}

