using System;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Persistence.DAL;
using HelloJobFinal.Persistence.Implementations.Repositories.Generic;

namespace HelloJobFinal.Persistence.Implementations.Repositories
{
    public class ExperienceRepository : Repository<Experience>, IExperienceRepository
    {
        public ExperienceRepository(AppDbContext context) : base(context) { }
    }
}


