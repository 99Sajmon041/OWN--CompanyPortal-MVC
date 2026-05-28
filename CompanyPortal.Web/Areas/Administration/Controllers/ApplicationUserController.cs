using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Authorization;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = AppRoles.MasterAdmin)]
public sealed class ApplicationUserController(IApplicationUserService applicationUserService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await applicationUserService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }


    [HttpGet]
    public async Task<IActionResult> Detail(string id, CancellationToken ct)
    {
        try
        {
            var result = await applicationUserService.GetInfoAsync(id, ct);
            return View(result);
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(string id, CancellationToken ct)
    {
        try
        {
            var date = await applicationUserService.ChangeStatusAsync(id, ct);
            TempData["Success"] = $"Uživatel úspěšně deaktivován k datu: {date}";
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(List));
    }
}