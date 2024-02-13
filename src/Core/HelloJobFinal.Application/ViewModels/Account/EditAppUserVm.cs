using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
	public record EditAppUserVm(string UserName, string Name, string Surname, string? Img)
    {
        public string? Id { get; init; }
        public IFormFile? Photo { get; init; }
    }
}

