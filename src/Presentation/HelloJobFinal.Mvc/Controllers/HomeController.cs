using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobFinal.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IVacancyService _vacancyService;
    private readonly IBaseCategoryService _baseCategoryService;
    private readonly ICompanyService _companyService;
    private readonly IVacancyWishlistService _vacancyWishlist;

    public HomeController(IBaseCategoryService baseCategoryService, IVacancyService vacancyService,
        ICompanyService companyService, IVacancyWishlistService vacancyWishlist)
    {
        _baseCategoryService = baseCategoryService;
        _vacancyService = vacancyService;
        _companyService = companyService;
        _vacancyWishlist = vacancyWishlist;
    }

    public async Task<IActionResult> Index()
    {
        HomeVm homeVm = new HomeVm
        {
            BaseCategoryVms = await _baseCategoryService.GetAllWhereAsync(11, 1),
            CompanyVms = await _companyService.GetAllWhereAsync(5, 1),
            VacancyVms = await _vacancyService.GetAllWhereAsync(15, 1),
            VacancyWishlists = await _vacancyWishlist.WishList()
        };  
        return View(homeVm);
    }
}

