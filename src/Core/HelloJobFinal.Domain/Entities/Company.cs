using System;
using HelloJobFinal.Domain.Enums;

namespace HelloJobFinal.Domain.Entities
{
	public class Company : BaseNameableEntity
	{
        public string ImageUrl { get; set; }
        public string Email { get; set; }

        //Relations
        public List<Vacancy> Vacancies { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Status Status { get; set; }
    }
}

