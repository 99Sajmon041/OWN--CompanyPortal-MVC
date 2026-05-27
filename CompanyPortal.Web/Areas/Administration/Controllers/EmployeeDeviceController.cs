using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Authorization;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = AppRoles.MasterAdmin)]
public class EmployeeDeviceController(IEmployeeDeviceService employeeDeviceService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await employeeDeviceService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }
}
