using System.Security.Claims;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloJobFinal.Mvc.Controllers
{
    [Authorize(Roles = "Employee, Company")]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly ICvService _cvService;
        private readonly IVacancyService _vacancyService;
        private readonly ICompanyService _companyService;
        private readonly IVacancyWishlistService _vacancyWishlist;
        private readonly ICvWishlistService _cvWishlist;



        public UserController(IUserService service, ICvService cvService, IVacancyService vacancyService,
            ICompanyService companyService, IVacancyWishlistService vacancyWishlist, ICvWishlistService cvWishlist)
        {
            _service = service;
            _cvService = cvService;
            _vacancyService = vacancyService;
            _companyService = companyService;
            _vacancyWishlist = vacancyWishlist;
            _cvWishlist = cvWishlist;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public async Task<IActionResult> EditUser(string id)
        {
            return View(await _service.EditUser(id));
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditAppUserVm editUser)
        {
            bool result = await _service.EditUserAsync(id, editUser, ModelState);
            if (!result)
            {
                return View(editUser);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> FogotPassword(string id)
        {
            await _service.ForgotPassword(id, Url);
            return View();
        }
        public IActionResult ChangePassword(string token)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string token, ChangePasswordVm fogotPassword)
        {
            bool result = await _service.ChangePassword(id, token, fogotPassword, ModelState);
            if (!result)
            {
                return View(fogotPassword);
            }
            return RedirectToAction(nameof(Index));
        }

        //Cv pages
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CvIndex()
        {

            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> CvWishList()
        {
            return View(await _cvWishlist.WishList());
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateCv()
        {
            CreateCvVm create = new CreateCvVm();
            await _cvService.CreatePopulateDropdowns(create);
            return View(create);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCv(CreateCvVm cvVm)
        {
            bool result = await _cvService.CreateAsync(cvVm, ModelState);
            if (!result)
            {
                return View(cvVm);
            }
            return RedirectToAction("Index", "User");
        }


        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateCv(int id)
        {
            return View(await _cvService.UpdateAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCv(int id, UpdateCvVm update)
        {
            bool result = await _cvService.UpdatePostAsync(id, update, ModelState);
            if (!result)
            {
                return View(update);
            }
            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> SoftDeleteCv(int id)
        {
            await _cvService.SoftDeleteAsync(id);

            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> AddCvRequest(int id)
        {
            await _cvService.AddCvRequestAsync(id, TempData);
            return RedirectToAction("detail", "cv", new { Id = id });
        }

        public async Task<IActionResult> AcceptCvRequest(int requestId)
        {
            await _cvService.AcceptCvRequestAsync(requestId);
            return RedirectToAction("MyOrder", "User");
        }

        public async Task<IActionResult> DeleteCvRequest(int requestId)
        {
            await _cvService.DeleteCvRequestAsync(requestId);
            return RedirectToAction("MyOrder", "User");
        }

        public async Task<IActionResult> MyCvOrder()
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public async Task<IActionResult> MyCvRequest()
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        //Vacancy pages

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> VacancyIndex(string id)
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> VacancyWishList()
        {
            return View(await _vacancyWishlist.WishList());
        }


        [Authorize(Roles = "Company")]
        public async Task<IActionResult> CreateVacancy()
        {
            CreateVacancyVm create = new CreateVacancyVm();
            await _vacancyService.CreatePopulateDropdowns(create);
            return View(create);
        }



        [HttpPost]
        public async Task<IActionResult> CreateVacancy(CreateVacancyVm vacancyVm)
        {
            bool result = await _vacancyService.CreateAsync(vacancyVm, ModelState);
            if (!result)
            {
                return View(vacancyVm);
            }
            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> UpdateVacancy(int id)
        {
            return View(await _vacancyService.UpdateAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVacancy(int id, UpdateVacancyVm update)
        {
            bool result = await _vacancyService.UpdatePostAsync(id, update, ModelState);
            if (!result)
            {
                return View(update);
            }
            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> SoftDeleteVacancy(int id)
        {
            await _vacancyService.SoftDeleteAsync(id);

            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> AddVacancyRequest(int id)
        {
            await _vacancyService.AddVacancyRequestAsync(id,TempData);
            return RedirectToAction("detail", "vacancy", new { Id = id });
        }

        public async Task<IActionResult> AcceptVacancyRequest(int requestId)
        {
            await _vacancyService.AcceptVacancyRequestAsync(requestId);
            return RedirectToAction("MyOrder", "User");
        }

        public async Task<IActionResult> DeleteVacancyRequest(int requestId)
        {
            await _vacancyService.DeleteVacancyRequestAsync(requestId);
            return RedirectToAction("MyOrder", "User");
        }

        public async Task<IActionResult> MyVacancyOrder()
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public async Task<IActionResult> MyVacancyRequest()
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }


        //Company

        [Authorize(Roles = "Company")]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> CompanyIndex(string id)
        {
            return View(await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CreateCompanyVm companyVm)
        {
            bool result = await _companyService.CreateAsync(companyVm, ModelState);
            if (!result)
            {
                return View(companyVm);
            }
            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            return View(await _companyService.UpdateAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCompany(int id, UpdateCompanyVm update)
        {
            bool result = await _companyService.UpdatePostAsync(id, update, ModelState);
            if (!result)
            {
                return View(update);
            }
            return RedirectToAction("CompanyIndex", "User");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> SoftDeleteCompany(int id)
        {
            await _companyService.SoftDeleteAsync(id);

            return RedirectToAction("Index", "User");
        }


    }
}

