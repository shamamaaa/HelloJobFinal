using System;
namespace HelloJobFinal.Domain.Entities
{
    public class Education : BaseNameableEntity
    {
        public List<Cv>? Cvs { get; set; }
    }
}

