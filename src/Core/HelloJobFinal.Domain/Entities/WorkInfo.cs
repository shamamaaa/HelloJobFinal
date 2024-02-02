using System;
namespace HelloJobFinal.Domain.Entities
{
	public class WorkInfo : BaseEntity
	{
        public string? Info { get; set; }
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}

