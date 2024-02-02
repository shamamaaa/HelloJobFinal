using System;
namespace HelloJobFinal.Domain.Entities
{
	public class City : BaseNameableEntity
	{
        public List<Cv>? Cvs { get; set; }
    }
}

