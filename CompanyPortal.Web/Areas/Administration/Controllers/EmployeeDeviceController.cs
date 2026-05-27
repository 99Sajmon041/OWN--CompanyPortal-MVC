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
public class EmployeeDeviceController(IEmployeeDeviceService employeeDeviceService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await employeeDeviceService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignDevice(int employeeId, int deviceId, string note, CancellationToken ct)
    {
        try
        {
            await employeeDeviceService.AssignDeviceAsync(employeeId, deviceId, note, ct);
            return RedirectToAction("Detail", "Employee", new { id = employeeId });
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Employee", "List");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Detail", "Employee", new { id = employeeId });
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnDevice(int employeeId, int employeeDeviceId, CancellationToken ct)
    {
        try
        {
            await employeeDeviceService.MarkAsReturnedAsync(employeeDeviceId, ct);
            return RedirectToAction("Detail", "Employee", new { id = employeeId });
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("List", "Employee");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Detail", "Employee", new { id = employeeId });
        }
    }
}
