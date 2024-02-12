using a=HelloJobFinal.Domain.Entities;
namespace HelloJobFinal.Application.ViewModels
{
    public record ItemCategoryItemVm(int Id, string Name, int BaseCategoryId, IncludeBaseCategoryVm BaseCategory)
    {
        public List<a.Cv>? Cvs { get; init; }
        public List<a.Vacancy>? Vacancies { get; init; }
    }
}

