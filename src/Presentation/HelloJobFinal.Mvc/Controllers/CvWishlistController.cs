using HelloJobFinal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;


namespace HelloJobFinal.Mvc.Controllers
{
    public class CvWishlistController : Controller
    {
        private readonly ICvWishlistService _service;

        public CvWishlistController(ICvWishlistService service)
        {
            _service = service;
        }

        public async Task<IActionResult> AddWishList(int id)
        {
            await _service.AddWishList(id);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            await _service.DeleteItem(id);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}

