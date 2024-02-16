namespace HelloJobFinal.Application.ViewModels
{
    public record ItemEducationVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }
        public ICollection<IncludeVacancyVm> includeVacancyVms { get; init; }
    }

}

