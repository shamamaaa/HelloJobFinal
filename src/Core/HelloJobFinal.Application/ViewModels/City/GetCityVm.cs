using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetCityVm(int Id, string Name)
    {
        public List<IncludeCvVm> Cvs { get; init; }

    }
}

