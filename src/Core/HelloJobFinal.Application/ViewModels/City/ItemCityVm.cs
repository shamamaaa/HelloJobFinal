using System;
namespace HelloJobFinal.Application.ViewModels.City
{
    public record ItemCityVm(int Id, string Name, List<IncludeCvVm> IncludeCvs);
}

