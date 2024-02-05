using System;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.ViewModels.Experience
{
    public record ItemExperienceVm(int Id,string Name, ICollection<IncludeCvVm> IncludeCvVms);

}

