namespace HelloJobFinal.Application.ViewModels
{
    public record GetEducationVm(int Id, string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }
        public ICollection<IncludeVacancyVm> includeVacancyVms { get; init; }


    }

}

