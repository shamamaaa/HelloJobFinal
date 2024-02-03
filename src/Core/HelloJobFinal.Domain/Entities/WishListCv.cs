using System;
namespace HelloJobFinal.Domain.Entities
{
	public class WishListCv : BaseEntity
	{
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public int CvId { get; set; }
        public Cv Cv { get; set; }
    }
}

