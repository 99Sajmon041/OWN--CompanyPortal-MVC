using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Controllers;

[Authorize]
public sealed class HomeController(IEmployeeService employee) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        try
        {
            var profile = await employee.GetEmployeeProfileAsync(ct);
            return View(profile);
        }
        catch (EntityNotFoundException ex)
        {
            return RedirectToAction("Status", "Error", new { code = 404, message = ex.Message });
        }
    }
}