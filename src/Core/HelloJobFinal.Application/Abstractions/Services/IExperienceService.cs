using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IExperienceService
	{
        Task<ICollection<ItemExperienceVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemExperienceVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Experience, object>>? orderExpression, int page = 1);
        Task<PaginationVm<ItemExperienceVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemExperienceVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetExperienceVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateExperienceVm create, ModelStateDictionary model);
        Task<UpdateExperienceVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateExperienceVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

