using System.Linq.Expressions;
using HelloJobFinal.Application.Abstractions.Repositories.Generic;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.Abstractions.Repositories
{
    public interface IVacancyRepository : IRepository<Vacancy>
    {
        void AddInfoWorks(Vacancy vacancy, string workInfo);
        void AddInfoEmployeers(Vacancy vacancy, string employeeInfo);
        Task<VacancyRequest> GetByIdVacancyRequest(int id);
        Task AddVacancyRequest(VacancyRequest item);
        void UpdateVacancyRequest(VacancyRequest item);
        void DeleteVacancyRequest(VacancyRequest item);
        Task<bool> CheckUniqueVacancyRequestAsync(Expression<Func<VacancyRequest, bool>> expression);
    }
}





