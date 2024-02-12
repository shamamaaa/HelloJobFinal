using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IBaseCategoryService
	{
        Task<ICollection<ItemBaseCategoryVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemBaseCategoryVm>> GetAllWhereByOrderAsync(int take, Expression<Func<BaseCategory, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemBaseCategoryVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemBaseCategoryVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetBaseCategoryVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateBaseCategoryVm create, ModelStateDictionary model);
        Task<UpdateBaseCategoryVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateBaseCategoryVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

