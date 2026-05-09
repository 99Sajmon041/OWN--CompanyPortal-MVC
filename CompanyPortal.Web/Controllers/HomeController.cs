using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
