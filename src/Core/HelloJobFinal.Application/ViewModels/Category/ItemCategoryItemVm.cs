using System;
namespace HelloJobFinal.Application.ViewModels.Category
{
    public record ItemCategoryItemVm(int Id, string Name, int BaseCategoryId, IncludeBaseCategoryVm BaseCategory);
}

