using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels.Category
{
    public record GetBaseCategoryVm(int Id, string Name, string ImageUrl)
    {
        public List<IncludeCategoryItemVm>? CategoryItems { get; init; }
    }
}

