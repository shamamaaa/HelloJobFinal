using HelloJobFinal.Application.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace HelloJobFinal.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterVM register, ModelStateDictionary model, IUrlHelper url);
        Task<bool> LogInAsync(LoginVM login, ModelStateDictionary model);
        Task LogOutAsync();
    }
}

