namespace HelloJobFinal.Application.ViewModels
{
    public record GetCompanyVm(int Id, string ImageUrl, string Email, string AppUserId, string Status, string Name)
    {
        public ICollection<IncludeVacancyVm> Vacancies { get; init; }
    }
}

