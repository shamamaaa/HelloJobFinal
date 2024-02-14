
using System;
namespace HelloJobFinal.Application.ViewModels
{
    public record LoginVM
    {
        public string UserNameOrEmail { get; init; }
        public string Password { get; init; }
        public bool IsRemembered { get; init; }
    }
}

