using System;
using HelloJobFinal.Application.ViewModels.Setting;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ISettingService
    {
        Task<ICollection<ItemSettingVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemSettingVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Setting, object>>? orderExpression, int page = 1);
        Task<GetSettingVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateSettingVm create, ModelStateDictionary model);
        Task<UpdateSettingVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateSettingVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

