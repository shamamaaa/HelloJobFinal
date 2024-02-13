﻿using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterVM register, ModelStateDictionary model, IUrlHelper url);
        Task<bool> LogInAsync(LoginVM login, ModelStateDictionary model);
        Task LogOutAsync();
        Task<bool> ConfirmEmail(string token, string email);
        Task<bool> ForgotPassword(FindAccountVm account, ModelStateDictionary model, IUrlHelper url);
        Task<bool> ChangePassword(string userNameOrEmail, string token, ForgotPasswordVm fogotPassword, ModelStateDictionary model);
    }
}

