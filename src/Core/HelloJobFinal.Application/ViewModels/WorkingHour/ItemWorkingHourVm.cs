namespace HelloJobFinal.Application.ViewModels
{
    public record ItemWorkingHourVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public ICollection<IncludeCvVm> IncludeCvVms { get; set; }
        public ICollection<IncludeVacancyVm> IncludeVacancyVms { get; set; }
    }

}

