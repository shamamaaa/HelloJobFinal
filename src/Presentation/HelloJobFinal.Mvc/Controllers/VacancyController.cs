using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;


namespace HelloJobFinal.Mvc.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        public async Task<IActionResult> Detail(int id, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _vacancyService.GetByIdAsync(id));
        }
    }
}

