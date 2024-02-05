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
        Task<GetExperienceVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateExperienceVm create, ModelStateDictionary model);
        Task<UpdateExperienceVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateExperienceVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

