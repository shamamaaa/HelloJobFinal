using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface ICategoryItemService
    {
        Task<ICollection<ItemCategoryItemVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCategoryItemVm>> GetAllWhereByOrderAsync(int take, Expression<Func<CategoryItem, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemCategoryItemVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemCategoryItemVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetCategoryItemVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCategoryItemVm create, ModelStateDictionary model);
        Task<UpdateCategoryItemVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCategoryItemVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }

}

