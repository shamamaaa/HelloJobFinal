using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterVM register, ITempDataDictionary tempData, IUrlHelper url);
        Task<bool> LogInAsync(LoginVM login, ITempDataDictionary tempData);
        Task LogOutAsync();
        Task<bool> ConfirmEmail(string token, string email);
        Task<bool> ForgotPassword(FindAccountVm account, ModelStateDictionary model, IUrlHelper url);
        Task<bool> ChangePassword(string id, ChangePasswordVm fogotPassword, ModelStateDictionary model);
        Task<bool> ResetPassword(string id, string token, ResetPasswordVm resetPassword, ModelStateDictionary model);
    }
}

