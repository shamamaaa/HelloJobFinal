using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Application.ViewModels.Category;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IBaseCategoryService
	{
        Task<ICollection<ItemBaseCategoryVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemBaseCategoryVm>> GetAllWhereByOrderAsync(int take, Expression<Func<BaseCategory, object>>? orderExpression, int page = 1);
        Task<GetBaseCategoryVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateBaseCategoryVm create, ModelStateDictionary model);
        Task<UpdateBaseCategoryVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateBaseCategoryVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

