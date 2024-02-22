using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace HelloJobFinal.Mvc.Controllers
{
    public class CompanyController : Controller
    {

        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(string? search, int order = 1, int page = 1)
        {
            return View(await _companyService.GetFilteredAsync(search, 6, page, order));
        }

        public async Task<IActionResult> Detail(int id)
        {
             return View(await _companyService.GetByIdAsync(id));
        }
    }
}

