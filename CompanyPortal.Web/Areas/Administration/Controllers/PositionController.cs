using CompanyPortal.Application.Models.Position;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "MasterAdmin, HR, ManagingDirector, ExecutiveManager")]
public sealed class PositionController(IPositionService positionService) : Controller
{
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await positionService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id, CancellationToken ct)
    {
        try
        {
            var result = await positionService.GetDetailAsync(id, ct);
            return View(result);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, CancellationToken ct)
    {
        try
        {
            string term = await positionService.ChangeStatusAsync(id, ct);
            TempData["Success"] = $"{term} proběhla úspěšně.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(List));
    }


    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var result = await positionService.GetForCreateAsync(ct);
        return View(result);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PositionUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await positionService.GetForCreateAsync(ct);
            model.Departments = result.Departments;

            return View(model);
        }

        try
        {
            await positionService.CreateAsync(model, ct);
            TempData["Success"] = "Pozice byla úspěšně vytvořena.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var result = await positionService.GetForCreateAsync(ct);
            model.Departments = result.Departments;

            return View(model);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id, CancellationToken ct)
    {
        try
        {
            var result = await positionService.GetForUpdateAsync(id, ct);
            return View(result);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(List));
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, PositionUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await positionService.GetForUpdateAsync(id, ct);
            model.Departments = result.Departments;

            return View(model);
        }

        try
        {
            await positionService.UpdateAsync(id, model, ct);
            TempData["Success"] = "Pozice byla úspěšně upravena.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var result = await positionService.GetForUpdateAsync(id, ct);
            model.Departments = result.Departments;

            return View(model);
        }
    }
}
