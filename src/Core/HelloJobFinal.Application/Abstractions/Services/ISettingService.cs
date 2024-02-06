using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Setting;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface ISettingService
    {
        Task<PaginationVm<ItemSettingVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<UpdateSettingVm> UpdateAsync(int id);
        Task<bool> UpdatePostAsync(int id, UpdateSettingVm update, ModelStateDictionary model);
    }
}

