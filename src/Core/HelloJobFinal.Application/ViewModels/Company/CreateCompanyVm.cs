using System;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Company
{
    public record CreateCompanyVm(IFormFile Photo, string Email, string AppUserId, string Status);
}

