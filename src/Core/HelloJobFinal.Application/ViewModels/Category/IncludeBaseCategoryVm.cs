using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeBaseCategoryVm(int Id,string Name, string ImageUrl)
    {
        public List<CategoryItem>? CategoryItems { get; init; }
    }
}

