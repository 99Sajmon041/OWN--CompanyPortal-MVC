using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Shared.Authorization;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = AppRoles.MasterAdmin + ", " + AppRoles.HRManager)]
public sealed class DepartmentController(IDepartmentService departmentService) : Controller
{
    [HttpGet]
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
        catch (EntityNotFoundException ex)
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
            string term = await departmentService.ChangeStatusAsync(id, ct);
            TempData["Success"] = $"{term} proběhla úspěšně.";
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


    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var result = await departmentService.GetForCreateAsync(ct);
        return View(result);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await departmentService.GetForCreateAsync(ct);
            model.Buildings = result.Buildings;

            return View(model);
        }

        try
        {
            await departmentService.CreateAsync(model, ct);
            TempData["Success"] = "Oddělení bylo úspěšně vytvořeno.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var result = await departmentService.GetForCreateAsync(ct);
            model.Buildings = result.Buildings;

            return View(model);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id, CancellationToken ct)
    {
        try
        {
            var result = await departmentService.GetForUpdateAsync(id, ct);
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
    public async Task<IActionResult> Update(int id, DepartmentUpsertViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await departmentService.GetForUpdateAsync(model.Id, ct);
            model.Buildings = result.Buildings;

            return View(model);
        }

        try
        {
            await departmentService.UpdateAsync(id, model, ct);
            TempData["Success"] = "Oddělení bylo úspěšně aktualizováno.";

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

            var result = await departmentService.GetForUpdateAsync(model.Id, ct);
            model.Buildings = result.Buildings;

            return View(model);
        }
    }
}