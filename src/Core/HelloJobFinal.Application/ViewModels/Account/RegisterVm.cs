using System;
using HelloJobFinal.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HelloJobFinal.Application.ViewModels.Account
{
    public record RegisterVM(string Name, string Surname, string UserName,
        string Email, string Password, string ConfirmPassword, string role, bool AllowTerms);
}

