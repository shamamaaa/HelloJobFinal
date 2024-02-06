using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.WorkingHour
{
    public record ItemWorkingHourVm(int Id,string Name, ICollection<IncludeCvVm> IncludeCvVms);

}

