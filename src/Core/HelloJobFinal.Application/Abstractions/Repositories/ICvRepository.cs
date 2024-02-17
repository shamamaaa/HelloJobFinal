using System;
using System.Linq.Expressions;
using HelloJobFinal.Application.Abstractions.Repositories.Generic;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.Abstractions.Repositories
{
	public interface ICvRepository : IRepository<Cv>
	{
        Task<CvRequest> GetByIdCvRequest(int id);
        Task AddCvRequest(CvRequest item);
        void UpdateCvRequest(CvRequest item);
        void DeleteCvRequest(CvRequest item);
        Task<bool> CheckUniqueCvRequestAsync(Expression<Func<CvRequest, bool>> expression);
    }
}

