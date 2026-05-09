using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService) : IEmployeeService
{
    public async Task<PagedResult<EmployeeViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<Employee> query = unitOfWork.EmployeeRepository.GetAllWithPositionAsync();

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query
            .ProjectTo<EmployeeViewModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return new PagedResult<EmployeeViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(Employee)
        };
    }

    public async Task<EmployeeViewModel?> GetByIdAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new Exception("Zaměstnanec nebyl nalezen.");
        }

        var deparmentVm = mapper.Map<EmployeeViewModel>(employee);

        return deparmentVm;
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new Exception("Zaměstnanec nebyl nalezen.");
        }

        string term;

        if (employee.IsActive)
        {
            employee.IsActive = false;
            term = "Deaktivace";
        }
        else
        {
            employee.IsActive = true;
            term = "Aktivace";
        }

        await unitOfWork.SaveChangesAsync(ct);

        return term;
    }

    public async Task<EmployeeUpsertViewModel> GetForCreate(CancellationToken ct)
    {
        var positions = await GetPositionsSelectListAsync(ct);
        var applicationUsers = await applicationUserService.GetUsersForSelectListAsync(ct);

        return new EmployeeUpsertViewModel
        {
            Positions = positions,
            ApplicationUsers = applicationUsers
        };
    }

    // - try catch add to Controller !!
    public async Task CreateAsync(EmployeeUpsertViewModel model, CancellationToken ct)
    {
        var employee = mapper.Map<Employee>(model);

        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        string randomPart = Random.Shared.Next(100, 1000).ToString();
        string personalNumber = $"{timeStamp}{randomPart}";
        personalNumber = personalNumber.Length > 16 ? personalNumber.Substring(0, 16) : personalNumber.PadLeft(16, '0');

        employee.PersonalNumber = personalNumber;
        employee.CreatedAt = DateTime.UtcNow;
        employee.IsActive = true;

        await unitOfWork.EmployeeRepository.AddAsync(employee, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await applicationUserService.MarkEmployeeProfileCompletedAsync(employee.ApplicationUserId, ct);
    }

    //Change FirstName and LastName allowed only, other properties will be disabled in the form
    public async Task<EmployeeUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new Exception("Zaměstnanec nebyl nalezen.");
        }

        var employeeVm = mapper.Map<EmployeeUpsertViewModel>(employee);

        return employeeVm;
    }

    public async Task UpdateAsync(int id, EmployeeUpsertViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new Exception("Neplatný identifikátor zaměstnance.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new Exception("Zaměstnanec nebyl nalezen.");
        }

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;

        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<List<SelectListItem>> GetPositionsSelectListAsync(CancellationToken ct)
    {
        var positions = await unitOfWork.PositionRepository.GetAllAsyncWithPositions(ct);

        return positions.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = $" {x.Department.DepartmentName}: {x.PositionName}"
        })
        .ToList();
    }
}
