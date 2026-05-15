using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Controllers;

[Authorize]
public sealed class ErrorController : Controller
{
    [Route("Error/StatusCode")]
    public IActionResult Status(int code, string? message)
    {
        ViewData["StatusCode"] = code;
        ViewData["ExceptionMessage"] = message;

        return View();
    }

    [Route("Error/Unexpected")]
    public IActionResult Unexpected()
    {
        return View();
    }
}
 