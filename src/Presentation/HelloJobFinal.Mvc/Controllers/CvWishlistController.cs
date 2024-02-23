using System.Data;
using HelloJobFinal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HelloJobFinal.Mvc.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CvWishlistController : Controller
    {
        private readonly ICvWishlistService _service;

        public CvWishlistController(ICvWishlistService service)
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
        public async Task<IActionResult> CvWishList()
        {
            return View(await _service.WishList());
        }
    }
}

