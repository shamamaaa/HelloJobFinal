using System;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IUserService
	{
        Task<PaginationVm<ItemAppUserVm>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<PaginationVm<ItemAppUserVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order);
        Task<GetAppUserVM> GetByIdAdminAsync(string id);
        Task<GetAppUserVM> GetByIdAsync(string id);
        Task<GetAppUserVM> GetByUserNameAdminAsync(string userName);
        Task<GetAppUserVM> GetByUserNameAsync(string userName);
        Task ReverseSoftDeleteAsync(string id);
        Task SoftDeleteAsync(string id);
        Task DeleteAsync(string id);
        Task GiveRoleModeratorAsync(string id);
        Task DeleteRoleModeratorAsync(string id);
        //Task<EditAppUserVm> EditUser(string id);
        //Task<bool> EditUserAsync(string id, EditAppUserVm update, ModelStateDictionary model);
        Task ForgotPassword(string id, IUrlHelper url);
        Task<bool> ChangePassword(string id, string token, ForgotPasswordVm fogotPassword, ModelStateDictionary model);
    }
}

