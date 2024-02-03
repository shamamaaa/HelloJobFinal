using System;
using HelloJobFinal.Domain.Enums;

namespace HelloJobFinal.Domain.Entities
{
	public class VacancyRequest : BaseEntity
	{
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public Status Status { get; set; }
    }
    
}

