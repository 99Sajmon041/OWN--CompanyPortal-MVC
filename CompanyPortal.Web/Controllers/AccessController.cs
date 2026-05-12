using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Controllers;

[Authorize]
public sealed class AccessController : Controller
{
    public IActionResult EmployeeProfileRequired()
    {
        return View();
    }

    public IActionResult EmployeeProfileNotActive()
    {
        return View();
    }
}
