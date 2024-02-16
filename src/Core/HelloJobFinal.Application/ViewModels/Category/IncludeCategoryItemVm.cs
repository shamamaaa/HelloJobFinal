using a=HelloJobFinal.Domain.Entities;
namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeCategoryItemVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int BaseCategoryId { get; init; }

        public IncludeBaseCategoryVm IncludeBaseCategory { get; set; }
        public List<a.Cv>? Cvs { get; init; }
        public List<a.Vacancy>? Vacancies { get; init; }
    }
}

