using System;
using System.Linq.Expressions;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Persistence.DAL;
using HelloJobFinal.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Repositories
{
    public class CvRepository : Repository<Cv>, ICvRepository
    {
        private readonly DbSet<CvRequest> _dbCvRequests;
        public CvRepository(AppDbContext context) : base(context)
        {
            _dbCvRequests = context.Set<CvRequest>();
        }
        public async Task<CvRequest> GetByIdCvRequest(int id)
        {
            return await _dbCvRequests.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddCvRequest(CvRequest item)
        {
            await _dbCvRequests.AddAsync(item);
        }
        public void UpdateCvRequest(CvRequest item)
        {
            _dbCvRequests.Update(item);
        }
        public void DeleteCvRequest(CvRequest item)
        {
            _dbCvRequests.Remove(item);
        }
        public async Task<bool> CheckUniqueCvRequestAsync(Expression<Func<CvRequest, bool>> expression)
        {
            return await _dbCvRequests.AnyAsync(expression);
        }
    }
}


