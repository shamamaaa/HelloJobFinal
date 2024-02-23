using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetBaseCategoryVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }

        public List<int>? VacancyIds { get; set; }
        public List<IncludeCategoryItemVm>? CategoryItems { get; init; }
    }
}

