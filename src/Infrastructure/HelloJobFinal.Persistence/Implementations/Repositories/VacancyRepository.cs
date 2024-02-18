using System;
using System.Linq.Expressions;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Persistence.DAL;
using HelloJobFinal.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Repositories
{
    public class VacancyRepository : Repository<Vacancy>, IVacancyRepository
    {
        private readonly DbSet<VacancyRequest> _dbVacancyRequests;
        private readonly DbSet<Requirement> _dbRequirements;
        private readonly DbSet<WorkInfo> _dbworkInfos;

        public VacancyRepository(AppDbContext context) : base(context)
        {
            _dbVacancyRequests = context.Set<VacancyRequest>();
            _dbworkInfos = context.Set<WorkInfo>();
            _dbRequirements = context.Set<Requirement>();
        }
        public async Task<VacancyRequest> GetByIdVacancyRequest(int id)
        {
            return await _dbVacancyRequests.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddVacancyRequest(VacancyRequest item)
        {
            await _dbVacancyRequests.AddAsync(item);
        }
        public void UpdateVacancyRequest(VacancyRequest item)
        {
            _dbVacancyRequests.Update(item);
        }
        public void DeleteVacancyRequest(VacancyRequest item)
        {
            _dbVacancyRequests.Remove(item);
        }
        public async Task<bool> CheckUniqueVacancyRequestAsync(Expression<Func<VacancyRequest, bool>> expression)
        {
            return await _dbVacancyRequests.AnyAsync(expression);
        }

        public async void AddInfoWorks(Vacancy vacans, string workInfo)
        {
            string[] workInfoArray = workInfo?.Split('/') ?? new string[0];
            foreach (string info in workInfoArray)
            {
                WorkInfo infoWork = new WorkInfo
                {
                    Vacancy = vacans,
                    Info = info
                };

                await _dbworkInfos.AddAsync(infoWork);
            }
        }

        public async void AddInfoEmployeers(Vacancy vacans, string employeeInfo)
        {
            string[] employeeInfoArray = employeeInfo?.Split('/') ?? new string[0];
            foreach (string info in employeeInfoArray)
            {
                Requirement infoEmployeer = new Requirement
                {
                    Vacancy = vacans,
                    EmployeeRequirement = info
                };

                await _dbRequirements.AddAsync(infoEmployeer);
            }
        }
    }
}


