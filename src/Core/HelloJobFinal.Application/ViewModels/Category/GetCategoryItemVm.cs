using System;
namespace HelloJobFinal.Application.ViewModels.Category
{
    public record GetCategoryItemVm(int Id, string Name, int BaseCategoryId, IncludeBaseCategoryVm BaseCategory);

}

