using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeBaseCategoryVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }

        public List<CategoryItem>? CategoryItems { get; set; }
    }
}

