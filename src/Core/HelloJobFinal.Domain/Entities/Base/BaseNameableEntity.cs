using System;
namespace HelloJobFinal.Domain.Entities
{
	public class BaseNameableEntity : BaseEntity
	{
        public string Name { get; set; } = null!;
    }
}

