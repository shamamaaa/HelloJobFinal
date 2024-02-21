using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloJobFinal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloJobFinal.Mvc.Controllers
{
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
    }
}

