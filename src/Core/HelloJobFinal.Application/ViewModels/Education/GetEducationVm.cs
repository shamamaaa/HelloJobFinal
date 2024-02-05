using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.Education
{
    public record GetEducationVm(int Id, string Name, ICollection<IncludeCvVm> IncludeCvVms);

}

