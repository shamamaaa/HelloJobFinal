using System;
using HelloJobFinal.Domain.Enums;

namespace HelloJobFinal.Domain.Entities
{
	public class CvRequest : BaseEntity
	{
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }

        public int CvId { get; set; }
        public Cv Cv { get; set; }

        public string Status { get; set; }
    }
}

