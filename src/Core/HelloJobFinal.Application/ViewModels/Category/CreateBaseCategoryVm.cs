using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Category
{
    public record CreateBaseCategoryVm(string Name, IFormFile Photo);
}

