using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;


namespace HelloJobFinal.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IBaseCategoryService _baseCategoryService;

        public CategoryController(IBaseCategoryService baseCategoryService)
        {
            _baseCategoryService = baseCategoryService;
        }

        public async Task<IActionResult> Detail(int id, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _baseCategoryService.GetByIdAsync(id));
        }
    }
}

