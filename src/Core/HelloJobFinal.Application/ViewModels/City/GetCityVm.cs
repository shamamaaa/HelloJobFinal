using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.City
{
    public record GetCityVm(int Id, string Name)
    {
        public List<IncludeCvVm> Cvs { get; init; }

    }
}

