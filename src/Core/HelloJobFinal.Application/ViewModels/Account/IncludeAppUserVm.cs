using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeAppUserVm
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
    }

}

