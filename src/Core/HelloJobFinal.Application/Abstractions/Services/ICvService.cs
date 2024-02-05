using System;
using HelloJobFinal.Application.ViewModels.Cv;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{ 
	public interface ICvService
	{
        Task<ICollection<ItemCvVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCvVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Cv, object>>? orderExpression, int page = 1);
        Task<GetCvVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateCvVm create, ModelStateDictionary model);
        Task<UpdateCvVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCvVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

