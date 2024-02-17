using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IVacancyService
    {
        Task<ICollection<ItemVacancyVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemVacancyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Vacancy, object>>? orderExpression, int page = 1);
        Task<PaginationVm<VacancyFilterVM>> GetFilteredAsync(string? search, int take, int page, int order, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId);
        Task<PaginationVm<VacancyFilterVM>> GetDeleteFilteredAsync(string? search, int take, int page, int order, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId);
        Task<GetVacancyVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateVacancyVm create, ModelStateDictionary model);
        Task<UpdateVacancyVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateVacancyVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
        Task CreatePopulateDropdowns(CreateVacancyVm create);
        Task UpdatePopulateDropdowns(UpdateVacancyVm update);

        void AddInfoWorks(Vacancy vacancy, string workInfo);
        void AddInfoEmployeers(Vacancy vacancy, string employeeInfo);
    }
}

