using System;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateCompanyVm(IFormFile Photo, string Email, string Name);
}

