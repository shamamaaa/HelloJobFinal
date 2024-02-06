using System;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Persistence.DAL;
using HelloJobFinal.Persistence.Implementations.Repositories.Generic;

namespace HelloJobFinal.Persistence.Implementations.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }
    }
}


