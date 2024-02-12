namespace HelloJobFinal.Application.ViewModels
{
    public record GetWorkingHourVm(int Id, string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }
    }

}

