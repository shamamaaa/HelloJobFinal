using a=HelloJobFinal.Domain.Entities;
namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeCategoryItemVm(int Id,string Name, int BaseCategoryId, IncludeBaseCategoryVm IncludeBaseCategory)
    {
        public List<a.Cv>? Cvs { get; init; }
        public List<a.Vacancy>? Vacancies { get; init; }
    }
}

