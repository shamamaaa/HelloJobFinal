using System;
namespace HelloJobFinal.Domain.Entities
{
	public class Requirement : BaseEntity
	{
        public string? EmployeeRequirement { get; set; }
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}

