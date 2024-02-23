using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloJobFinal.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    [AutoValidateAntiforgeryToken]
    public class VacancyController : Controller
    {
        private readonly IVacancyService _service;

        public VacancyController(IVacancyService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId, int page = 1, int order = 1)
        {
            return View(model: await _service.GetFilteredAsync(search, 10, page, order, categoryId, cityId, educationId, experienceId, workingHourId));
        }

        public async Task<IActionResult> DeletedItems(string? search, int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId, int page = 1, int order = 1)
        {
            return View(model: await _service.GetDeleteFilteredAsync(search, 10, page, order, categoryId, cityId, educationId, experienceId, workingHourId));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await _service.UpdateAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateVacancyVm update)
        {
            bool result = await _service.UpdatePostAsync(id, update, ModelState);
            if (!result)
            {
                return View(update);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReverseSoftDelete(int id)
        {
            await _service.ReverseSoftDeleteAsync(id);

            return RedirectToAction(nameof(DeletedItems));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(DeletedItems));
        }

        public async Task<IActionResult> Detail(int id)
        {
            GetVacancyVm get = await _service.GetByIdAsync(id);

            return View(get);
        }
    }
}

