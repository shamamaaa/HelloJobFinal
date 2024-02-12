using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record CreateBaseCategoryVm(string Name, IFormFile Photo);
}

