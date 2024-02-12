namespace HelloJobFinal.Application.ViewModels
{
    public record GetExperienceVm(int Id, string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }

    }

}

