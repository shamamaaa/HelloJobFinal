using a=HelloJobFinal.Domain.Entities;
namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeCategoryItemVm(int Id,string Name, int BaseCategoryId)
    {
        public IncludeBaseCategoryVm IncludeBaseCategory { get; set; }
        public List<a.Cv>? Cvs { get; init; }
        public List<a.Vacancy>? Vacancies { get; init; }
    }
}

