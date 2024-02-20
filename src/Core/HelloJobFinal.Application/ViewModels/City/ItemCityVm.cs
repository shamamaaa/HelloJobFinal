using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record ItemCityVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public List<IncludeCvVm> Cvs { get; set; }
        public List<IncludeVacancyVm> Vacancies { get; set; }

    }
}

