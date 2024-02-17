using System.Linq.Expressions;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface ICvService
	{
        Task<ICollection<ItemCvVm>> GetAllWhereAsync(int take, int page = 1);
        Task<ICollection<ItemCvVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Cv, object>>? orderExpression, int page = 1);
        Task<PaginationVm<CvFilterVM>> GetFilteredAsync(string? search, int take, int page, int order, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId);
        Task<PaginationVm<CvFilterVM>> GetDeleteFilteredAsync(string? search, int take, int page, int order, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId);
        Task<GetCvVm> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCvVm create, ModelStateDictionary model);
        Task<UpdateCvVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateCvVm update, ModelStateDictionary model);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseSoftDeleteAsync(int id);
        Task CreatePopulateDropdowns(CreateCvVm create);
        Task UpdatePopulateDropdowns(UpdateCvVm update);
        Task<bool> AddCvRequestAsync(int id, ITempDataDictionary tempData);
        Task AcceptCvRequestAsync(int requestId);
        Task DeleteCvRequestAsync(int requestId);
    }
}

