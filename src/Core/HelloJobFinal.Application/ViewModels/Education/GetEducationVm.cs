namespace HelloJobFinal.Application.ViewModels
{
    public record GetEducationVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }
        public ICollection<IncludeVacancyVm> IncludeVacancyVms { get; init; }
    }

}

