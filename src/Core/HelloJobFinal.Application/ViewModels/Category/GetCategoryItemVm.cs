using a=HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetCategoryItemVm
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int BaseCategoryId { get; init; }
        public IncludeBaseCategoryVm BaseCategory { get; init; }

        public List<IncludeCvVm>? Cvs { get; set; }
        public List<IncludeVacancyVm>? Vacancies { get; set; }
    }

}

