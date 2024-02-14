using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloJobFinal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

