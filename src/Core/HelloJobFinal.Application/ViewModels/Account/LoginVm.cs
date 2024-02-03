
using System;
namespace HelloJobFinal.Application.ViewModels.Account
{
    public record LoginVM(string UserNameOrEmail, string Password, bool IsRemembered);
}

