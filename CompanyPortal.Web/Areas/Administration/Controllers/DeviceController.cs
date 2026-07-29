using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Device;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Authorization;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = AppRoles.MasterAdmin)]
public sealed class DeviceController(IDeviceService deviceService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await deviceService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }


    [HttpGet]
    public async Task<IActionResult> Detail(int id, CancellationToken ct)
    {
        try
        {
            var result = await deviceService.GetDetailAsync(id, ct);
            return View(result);
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }


    [HttpGet]
    public IActionResult Create()
    {
        var result = deviceService.GetForUpsert();
        return View(result);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DeviceUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            model.DeviceTypes = deviceService.GetForUpsert().DeviceTypes;
            return View(model);
        }

        try
        {
            await deviceService.CreateAsync(model, ct);

            TempData["Success"] = "Vytvoření zařízení proběhlo úspěšně.";
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            model.DeviceTypes = deviceService.GetForUpsert().DeviceTypes;

            return View(model);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id, CancellationToken ct)
    {
        try
        {
            var result = await deviceService.GetForUpdateAsync(id, ct);
            return View(result);
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, DeviceUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = deviceService.GetForUpsert();
            model.DeviceTypes = result.DeviceTypes;

            return View(model);
        }

        try
        {
            await deviceService.UpdateAsync(id, model, ct);
            TempData["Success"] = "Úprava zařízení proběhla úspěšně.";

            return RedirectToAction(nameof(List));
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var result = deviceService.GetForUpsert();
            model.DeviceTypes = result.DeviceTypes;

            return View(model);
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deprecate(int id, CancellationToken ct)
    {
        try
        {
            await deviceService.DeprecateAsync(id, ct);
            TempData["Success"] = "Nastavení zařízení jako zastaralé proběhlo úspěšně.";

            return RedirectToAction(nameof(List));
        }
        catch (EntityNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }
}
