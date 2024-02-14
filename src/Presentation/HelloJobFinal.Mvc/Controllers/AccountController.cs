﻿using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloJobFinal.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }



        public async Task<IActionResult> Login(LoginVM login, string? returnUrl)
        {
            bool result = await _service.LogInAsync(login, ModelState);
            if (!result)
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task<IActionResult> Register(RegisterVM register)
        {
            bool result = await _service.RegisterAsync(register, ModelState, Url);
            if (!result)
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            return RedirectToAction(nameof(SuccessfullyRegistred));
        }
        public async Task<IActionResult> LogOut()
        {
            await _service.LogOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        public IActionResult SuccessfullyRegistred()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            bool result = await _service.ConfirmEmail(token, email);
            if (!result)
            {
                return View();
            }
            return View();
        }
        public IActionResult FogotPasswordSended()
        {
            return View();
        }
        public IActionResult FogotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FogotPassword(FindAccountVm account)
        {
            bool result = await _service.ForgotPassword(account, ModelState, Url);
            if (!result)
            {
                return View(account);
            }
            return RedirectToAction(nameof(FogotPasswordSended));
        }
        public IActionResult ChangePassword(string id, string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string id, string token, ForgotPasswordVm fogotPassword)
        {
            bool result = await _service.ChangePassword(id, token, fogotPassword, ModelState);
            if (!result)
            {
                return View(fogotPassword);
            }
            return RedirectToAction(nameof(Login));
        }
    }
}

