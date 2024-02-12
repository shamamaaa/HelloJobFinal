namespace HelloJobFinal.Application.ViewModels
{
    public record ItemWorkingHourVm(int Id,string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }

    }

}

