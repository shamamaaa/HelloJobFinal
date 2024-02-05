using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IEducationService
	{
        Task<ICollection<ItemEducationVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemEducationVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Education, object>>? orderExpression, int page = 1);
        Task<GetEducationVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateEducationVm create, ModelStateDictionary model);
        Task<UpdateEducationVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateEducationVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

