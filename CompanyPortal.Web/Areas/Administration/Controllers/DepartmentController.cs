using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "MasterAdmin, HR, ManagingDirector, ExecutiveManager")]
public class DepartmentController(IDepartmentService departmentService) : Controller
{
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await departmentService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id, CancellationToken ct)
    {
        try
        {
            var result = await departmentService.GetDetailAsync(id, ct);
            return View(result);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }
}
