﻿using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobFinal.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    [AutoValidateAntiforgeryToken]
    public class BaseCategoryController : Controller
    {
        private readonly IBaseCategoryService _service;

        public BaseCategoryController(IBaseCategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? search, int order = 1, int page = 1)
        {
            return View(model: await _service.GetFilteredAsync(search, 10, page, order));
        }

        public async Task<IActionResult> DeletedItems(string? search, int order = 1, int page = 1)
        {
            return View(model: await _service.GetDeleteFilteredAsync(search, 10, page, order));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBaseCategoryVm create)
        {
            bool result = await _service.CreateAsync(create, ModelState);
            if (!result)
            {
                return View(create);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(await _service.UpdateAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBaseCategoryVm update)
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
            return View(await _service.GetByIdAsync(id));
        }
    }
}

