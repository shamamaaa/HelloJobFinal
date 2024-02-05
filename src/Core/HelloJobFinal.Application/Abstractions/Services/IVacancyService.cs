using System;
using HelloJobFinal.Application.ViewModels.Vacancy;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IVacancyService
    {
        Task<ICollection<ItemVacancyVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemVacancyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Vacancy, object>>? orderExpression, int page = 1);
        Task<GetVacancyVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateVacancyVm create, ModelStateDictionary model);
        Task<UpdateVacancyVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateVacancyVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

