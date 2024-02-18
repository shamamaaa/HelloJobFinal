using System;
using Microsoft.AspNetCore.Identity;

namespace HelloJobFinal.Domain.Entities
{
	public class AppUser : IdentityUser
	{
		public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActivate { get; set; }

        public List<Company>? Companies { get; set; }  //Company daxilinden vakansiyalara el catir
        public List<Cv>? Cvs { get; set; }  //Employee elan kimi bir nece cv yerelesdire bilermis.
		public List<WishListCv>? WishListCvs { get; set; }
        public List<Vacancy>? Vacancies { get; set; }

        public List<WishListVacancy>? WishListVacancies { get; set; }
        public List<CvRequest>? CvRequests { get; set; }
        public List<VacancyRequest>? VacancyRequests { get; set; }


    }
}

