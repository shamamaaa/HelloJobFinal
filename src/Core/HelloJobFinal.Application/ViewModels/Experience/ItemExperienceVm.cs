namespace HelloJobFinal.Application.ViewModels
{
    public record ItemExperienceVm(int Id,string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }

    }

}

