using System;
namespace HelloJobFinal.Domain.Entities
{
    public class WishListVacancy : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}

