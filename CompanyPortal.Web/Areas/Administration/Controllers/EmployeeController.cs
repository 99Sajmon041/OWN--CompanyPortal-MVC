using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Application.Services.Implementations;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Web.RouteHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = "MasterAdmin, HR, ManagingDirector, ExecutiveManager")]
public sealed class EmployeeController(IEmployeeService employeeService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        GridRequest gridRequest = GridRequestBuilder.GetFromRequest(Request);

        var result = await employeeService.GetAllPagedAsync(gridRequest, ct);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id, CancellationToken ct)
    {
        try
        {
            var result = await employeeService.GetDetailAsync(id, ct);
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
            string term = await employeeService.ChangeStatusAsync(id, ct);
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
        var result = await employeeService.GetForCreate(ct);
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await employeeService.GetForCreate(ct);

            model.ApplicationUsers = result.ApplicationUsers;
            model.Positions = result.Positions;

            return View(model);
        }

        try
        {
            await employeeService.CreateAsync(model, ct);
            TempData["Success"] = "Zaměstnanec byl úspěšně vytvořen.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            var result = await employeeService.GetForCreate(ct);

            model.ApplicationUsers = result.ApplicationUsers;
            model.Positions = result.Positions;

            ModelState.AddModelError(string.Empty, ex.Message);

            return View(model);
        }   
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id, CancellationToken ct)
    {
        try
        {
            var result = await employeeService.GetForUpdateAsync(id, ct);
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
    public async Task<IActionResult> Update(int id, EmployeeUpdateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await employeeService.GetForUpdateAsync(id, ct);
            return View(model);
        }

        try
        {
            await employeeService.UpdateAsync(id, model, ct);
            TempData["Success"] = "Zaměstnanec byl úspěšně upraven.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> PositionUpdate(int id, CancellationToken ct)
    {
        try
        {
            var result = await employeeService.GetForPositionUpdateAsync(id, ct);
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
    public async Task<IActionResult> PositionUpdate(int id, EmployeePositionUpdateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var result = await employeeService.GetForPositionUpdateAsync(id, ct);
            model.Positions = result.Positions;

            return View(model);
        }

        try
        {
            await employeeService.UpdatePositionAsync(id, model, ct);
            TempData["Success"] = "Pozice byla úspěšně upravena.";

            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}
