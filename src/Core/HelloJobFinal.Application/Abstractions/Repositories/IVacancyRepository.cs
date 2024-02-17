using HelloJobFinal.Application.Abstractions.Repositories.Generic;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.Abstractions.Repositories
{
    public interface IVacancyRepository : IRepository<Vacancy>
    {
        void AddInfoWorks(Vacancy vacancy, string workInfo);
        void AddInfoEmployeers(Vacancy vacancy, string employeeInfo);
    }
}





