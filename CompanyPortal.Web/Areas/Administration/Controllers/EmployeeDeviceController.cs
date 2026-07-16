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

            TempData["Success"] = "Zařízení bylo úspěšně zapůjčeno.";
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
    public async Task<IActionResult> ReturnDevice(int employeeId, int? employeeDeviceId, CancellationToken ct)
    {
        try
        {
            if (!employeeDeviceId.HasValue)
            {
                throw new Exception("Zařízení musí být vybráno.");
            }

            await employeeDeviceService.MarkAsReturnedAsync(employeeDeviceId.Value, ct);

            TempData["Success"] = "Zařízení bylo úspěšně vráceno.";
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
}
