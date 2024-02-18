namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeRequirement
	{
        public int Id { get; set; }
        public string? EmployeeRequirement { get; init; }
        public int VacancyId { get; init; }
        public IncludeVacancyVm Vacancy { get; init; }
    }
}

