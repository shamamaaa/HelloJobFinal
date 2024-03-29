﻿using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloJobFinal.Mvc.Controllers
{
    public class CvController : Controller
    {
        private readonly ICvService _cvService;

        public CvController(ICvService cvService)
        {
            _cvService = cvService;
        }

        public async Task<IActionResult> Detail(int id, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _cvService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Index(string? search,
            int? categoryItemId, int? educationId, int? experienceId, int? workingHourId, bool? hasDriverLicense, string? status,
            int page = 1, int order = 1)
        {
            return View(await _cvService
                .GetFilteredAsync(search, 10, page, order, status,categoryItemId,null,educationId, experienceId,workingHourId,hasDriverLicense));
        }
    }
}

