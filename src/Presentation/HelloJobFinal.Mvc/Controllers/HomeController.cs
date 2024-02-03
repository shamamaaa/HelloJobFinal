using Microsoft.AspNetCore.Mvc;

namespace HelloJobFinal.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

