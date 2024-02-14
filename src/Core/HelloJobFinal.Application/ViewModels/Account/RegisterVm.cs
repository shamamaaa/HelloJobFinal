using System;
using HelloJobFinal.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HelloJobFinal.Application.ViewModels
{
    public record RegisterVM
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
        public string Role { get; init; }
        public bool AllowTerms { get; init; }
    }
}

