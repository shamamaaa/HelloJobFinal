using System;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ICityService
    {
        Task<ICollection<ItemCityVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCityVm>> GetAllWhereByOrderAsync(int take, Expression<Func<City, object>>? orderExpression, int page = 1);
        Task<GetCityVm> GetByIdAsync(int id, int take, int page = 1);
        Task<bool> CreateAsync(CreateCityVm create, ModelStateDictionary model);
        Task<UpdateCityVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCityVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
    }
}

