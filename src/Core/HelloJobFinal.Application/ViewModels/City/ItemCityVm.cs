using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record ItemCityVm(int Id, string Name)
    {
        public List<IncludeCvVm> Cvs { get; init; }
    }
}

