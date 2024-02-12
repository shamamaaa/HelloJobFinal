using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IWorkingHourService
    {
        Task<ICollection<ItemWorkingHourVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemWorkingHourVm>> GetAllWhereByOrderAsync(int take, Expression<Func<WorkingHour, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemWorkingHourVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemWorkingHourVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetWorkingHourVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateWorkingHourVm create, ModelStateDictionary model);
        Task<UpdateWorkingHourVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateWorkingHourVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

