using System;
namespace HelloJobFinal.Domain.Entities
{
	public class BaseCategory : BaseNameableEntity
	{
        public string ImageUrl { get; set; }
        public List<CategoryItem>? CategoryItems { get; set; }
    }
}

