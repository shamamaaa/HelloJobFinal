using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCategoryItemVm(string Name, int BaseCategoryId)
    {
        public List<IncludeBaseCategoryVm> Categorys { get; set; }
    }
}

