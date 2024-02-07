using System;
namespace HelloJobFinal.Application.ViewModels.Category
{
    public record UpdateCategoryItemVm(string Name, int BaseCategoryId)
    {
        public List<IncludeBaseCategoryVm> Categorys { get; set; }

    }
}

