using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

public sealed class EmployeeController : Controller
{
    public IActionResult List()
    {
        return View();
    }
}
