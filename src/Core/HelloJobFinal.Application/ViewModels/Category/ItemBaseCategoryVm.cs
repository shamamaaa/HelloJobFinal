using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Category
{
    public record ItemBaseCategoryVm(int Id, string Name, string ImageUrl);
}
