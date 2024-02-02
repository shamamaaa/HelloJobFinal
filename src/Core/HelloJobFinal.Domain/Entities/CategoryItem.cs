using System;
namespace HelloJobFinal.Domain.Entities
{
	public class CategoryItem :BaseNameableEntity
	{
        public List<Cv>? Cvs { get; set; }
        public List<Vacancy>? Vacancies { get; set; }

        public int BaseCategoryId { get; set; }
        public BaseCategory BaseCategory { get; set; }
    }
}

