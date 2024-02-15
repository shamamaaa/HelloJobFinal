using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateBaseCategoryVm(string Name, string ImageUrl)
    {
        public IFormFile? Photo { get; init; }
    }

}

