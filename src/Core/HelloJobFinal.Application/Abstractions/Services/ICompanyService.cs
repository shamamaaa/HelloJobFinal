﻿using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ICompanyService
	{
        Task<ICollection<ItemCompanyVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCompanyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Company, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemCompanyVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemCompanyVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetCompanyVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCompanyVm create, ModelStateDictionary model);
        Task<UpdateCompanyVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCompanyVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

