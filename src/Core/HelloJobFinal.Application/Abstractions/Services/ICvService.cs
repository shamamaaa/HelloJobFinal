using System;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Cv;

namespace HelloJobFinal.Application.Abstractions.Services
{ 
	public interface ICvService
	{
        Task<ICollection<ItemCvVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCvVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Cv, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemCvVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemCvVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetCvVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCvVm create, ModelStateDictionary model);
        Task<UpdateCvVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCvVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
        Task CreatePopulateDropdowns(CreateCvVm create);
        Task UpdatePopulateDropdowns(UpdateCvVm update);
    }
}

