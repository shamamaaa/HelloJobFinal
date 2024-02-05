using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ICompanyService
	{
        Task<ICollection<ItemCompanyVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCompanyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Company, object>>? orderExpression, int page = 1);
        Task<GetCompanyVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateCompanyVm create, ModelStateDictionary model);
        Task<UpdateCompanyVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCompanyVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

