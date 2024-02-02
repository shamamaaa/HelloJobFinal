using System;
using HelloJobFinal.Domain.Enums;

namespace HelloJobFinal.Domain.Entities
{
	public class Cv : BaseNameableEntity
	{
		public string Surname { get; set; } //Name baseden gelir
		public string Email { get; set; }
		public DateTime Birthday { get; set; }
		public int Phone { get; set; }
		public string ImageUrl { get; set; }
		public string Position { get; set; }
		public int MinSalary { get; set; }
		public bool HasDriverLicense { get; set; }

        public DateTime FinishTime { get; set; } //Yaranmasi baseden gelir

        public string CvFile { get; set; } //pdf

        public bool IsTimeOver { get; set; } = false; //muddeti dolub yoxsa yox

        public int ViewCount { get; set; }  

        //Relations
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


        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public Status Status { get; set; }


        public List<WishListItem>? WishListItems { get; set; }

        //request ne edecem bilmirem
    }
}

