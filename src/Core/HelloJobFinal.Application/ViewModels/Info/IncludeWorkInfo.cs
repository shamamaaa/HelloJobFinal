namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeWorkInfo
	{
        public string? Info { get; init; }
        public int VacancyId { get; init; }
        public IncludeVacancyVm Vacancy { get; init; }
    }
}

