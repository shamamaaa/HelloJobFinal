using System;
using HelloJobFinal.Domain.Enums;

namespace HelloJobFinal.Domain.Entities
{
	public class Vacancy : BaseEntity
    {
        public string Position { get; set; }

        public int? Salary { get; set; }

        public bool HasDriverLicense { get; set; }

        public DateTime FinishTime { get; set; } //Yaranmasi baseden gelir

        public bool IsTimeOver { get; set; } = false; //muddeti dolub yoxsa yox

        public int ViewCount { get; set; }

        //Relations
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }

        public int ExperienceId { get; set; }
        public Experience Experience { get; set; }

        public int WorkingHourId { get; set; }
        public WorkingHour WorkingHour { get; set; }

        public int CategoryId { get; set; }
        public CategoryItem CategoryItem { get; set; }

        public string Status { get; set; }

        public List<Requirement>? Requirements { get; set; } //namized telebi
        public List<WorkInfo>? WorkInfos { get; set; } //ish melumati
        public List<VacancyRequest>? VacancyRequests { get; set; }
        public List<WishListVacancy>? WishListVacancies { get; set; }



    }
}

