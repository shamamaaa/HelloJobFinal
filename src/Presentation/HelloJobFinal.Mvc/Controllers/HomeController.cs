using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobFinal.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IVacancyService _vacancyService;
    private readonly ICategoryItemService _categoryItemService;
    private readonly IBaseCategoryService _baseCategoryService;

    private readonly ICompanyService _companyService;

    public HomeController(ICategoryItemService categoryItemService, IBaseCategoryService baseCategoryService)
    {
        _categoryItemService = categoryItemService;
        _baseCategoryService = baseCategoryService;
    }

    public async Task<IActionResult> Index()
    {
        //var vacancies = await _vacancyService.GetAllWhereAsync(10);
        //var companies = await _companyService.GetAllWhereAsync(10);
        var categories = await _categoryItemService.GetAllWhereAsync(10);
        var basecategories = await _baseCategoryService.GetAllWhereAsync(10);


        HomeVm homeVm = new HomeVm
        {
            CategoryItemVms = categories,
            BaseCategoryVms = basecategories
            //CompanyVms = companies,
            //VacancyVms = vacancies
        };  
        return View(homeVm);
    }
}

