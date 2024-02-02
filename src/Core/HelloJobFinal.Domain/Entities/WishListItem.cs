using System;
namespace HelloJobFinal.Domain.Entities
{
	public class WishListItem : BaseEntity
	{
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public int CvId { get; set; }
        public Cv Cv { get; set; }

        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public bool IsLiked { get; set; }

    }
}

