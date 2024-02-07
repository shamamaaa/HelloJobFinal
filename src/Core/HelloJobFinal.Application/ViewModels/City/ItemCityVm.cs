using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.City
{
    public record ItemCityVm(int Id, string Name)
    {
        public List<IncludeCvVm> Cvs { get; init; }
    }
}

