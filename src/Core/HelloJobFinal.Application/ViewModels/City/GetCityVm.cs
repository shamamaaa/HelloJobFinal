using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.City
{
    public record GetCityVm(int Id, string Name, List<IncludeCvVm> IncludeCvs);
}

