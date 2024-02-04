using System;
namespace HelloJobFinal.Application.ViewModels.City
{
    public record GetCityVm(int Id, string Name, List<IncludeCvVm> IncludeCvs);
}

