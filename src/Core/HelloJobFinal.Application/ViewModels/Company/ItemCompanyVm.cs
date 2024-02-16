namespace HelloJobFinal.Application.ViewModels
{
    public record ItemCompanyVm
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string AppUserId { get; init; }
        public string Status { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public ICollection<IncludeVacancyVm> Vacancies { get; init; }
    }

}

