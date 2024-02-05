using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Account
{
    public record IncludeAppUserVm(string Name, string Surname, string Email);

}

