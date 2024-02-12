using System;
namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateCategoryItemVm(string Name, int BaseCategoryId)
    {
        public List<IncludeBaseCategoryVm> Categorys { get; set; }

    }
}

