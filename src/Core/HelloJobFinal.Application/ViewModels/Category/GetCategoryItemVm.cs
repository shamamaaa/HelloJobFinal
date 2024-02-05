using a=HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels.Category
{
    public record GetCategoryItemVm(int Id, string Name, int BaseCategoryId, IncludeBaseCategoryVm BaseCategory)
    {
        public List<a.Cv>? Cvs { get; init; }
        public List<a.Vacancy>? Vacancies { get; init; }
    }

}

