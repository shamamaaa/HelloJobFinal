using System;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record ItemBaseCategoryVm(int Id, string Name, string ImageUrl)
    {
        public List<IncludeCategoryItemVm>? CategoryItems { get; init; }
    }
}
