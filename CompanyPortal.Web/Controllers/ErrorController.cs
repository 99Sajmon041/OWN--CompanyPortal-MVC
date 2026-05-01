using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Controllers;

[AllowAnonymous]
public sealed class ErrorController : Controller
{
    [Route("Error/StatusCode")]
    public IActionResult Status(int code)
    {
        ViewData["StatusCode"] = code;

        return View();
    }

    [Route("Error/Unexpected")]
    public IActionResult Unexpected()
    {
        return View();
    }
}
 