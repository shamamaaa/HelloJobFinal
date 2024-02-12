using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface ICityService
    {
        Task<ICollection<ItemCityVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCityVm>> GetAllWhereByOrderAsync(int take, Expression<Func<City, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemCityVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemCityVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetCityVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCityVm create, ModelStateDictionary model);
        Task<UpdateCityVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCityVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

