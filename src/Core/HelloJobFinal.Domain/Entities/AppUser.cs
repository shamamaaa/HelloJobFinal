using System;
using Microsoft.AspNetCore.Identity;

namespace HelloJobFinal.Domain.Entities
{
	public class AppUser : IdentityUser
	{
		public string Name { get; set; }
        public string Surname { get; set; }
		public List<Company>? Companies { get; set; }  //Company daxilinden vakansiyalara el catir
        public List<Cv>? Cvs { get; set; }  //Employee elan kimi bir nece cv yerelesdire bilermis.
		public List<WishListItem> WishListItems { get; set; }
	}
}

