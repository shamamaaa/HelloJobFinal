using System;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record ItemBaseCategoryVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }

        public List<IncludeCategoryItemVm>? CategoryItems { get; init; }
    }
}
