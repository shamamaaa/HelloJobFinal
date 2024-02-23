using HelloJobFinal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HelloJobFinal.Mvc.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class VacancyWishlistController : Controller
    {
        private readonly IVacancyWishlistService _service;

        public VacancyWishlistController(IVacancyWishlistService service)
        {
            _service = service;
        }

        public async Task<IActionResult> AddWishList(int id, string? returnUrl)
        {
            await _service.AddWishList(id);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task<IActionResult> DeleteItem(int id, string? returnUrl)
        {
            await _service.DeleteItem(id);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task<IActionResult> VacancyWishList()
        {
            return View(await _service.WishList());
        }
    }
}

