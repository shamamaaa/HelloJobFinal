
using System;
namespace HelloJobFinal.Application.ViewModels
{
    public record LoginVM(string UserNameOrEmail, string Password, bool IsRemembered);
}

