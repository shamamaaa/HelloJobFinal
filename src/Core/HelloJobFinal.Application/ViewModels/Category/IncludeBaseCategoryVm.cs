using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels.Category
{
    public record IncludeBaseCategoryVm(string Name, string ImageUrl)
    {
        public List<CategoryItem>? CategoryItems { get; init; }
    }
}

