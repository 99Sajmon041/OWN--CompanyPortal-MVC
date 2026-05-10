using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
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

    public async Task<EmployeeViewModel?> GetDetailAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var deparmentVm = mapper.Map<EmployeeViewModel>(employee);

        return deparmentVm;
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
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

    public async Task<EmployeeCreateViewModel> GetForCreate(CancellationToken ct)
    {
        var positions = await GetPositionsSelectListAsync(ct);
        var applicationUsers = await applicationUserService.GetUsersForSelectListAsync(ct);

        return new EmployeeCreateViewModel
        {
            Positions = positions,
            ApplicationUsers = applicationUsers
        };
    }

    public async Task CreateAsync(EmployeeCreateViewModel model, CancellationToken ct)
    {
        var employee = mapper.Map<Employee>(model);

        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        string randomPart = Random.Shared.Next(100, 1000).ToString();
        string personalNumber = $"{timeStamp}{randomPart}";
        personalNumber = personalNumber.Length > 16 ? personalNumber[..16] : personalNumber.PadLeft(16, '0');

        employee.PersonalNumber = personalNumber;
        employee.CreatedAt = DateTime.UtcNow;
        employee.IsActive = true;

        await using var transaction = await unitOfWork.BeginTransactionAsync(ct);

        try
        {
            await unitOfWork.EmployeeRepository.AddAsync(employee, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await applicationUserService.MarkEmployeeProfileCompletedAsync(employee.ApplicationUserId, ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<EmployeeUpdateViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var employeeVm = mapper.Map<EmployeeUpdateViewModel>(employee);

        return employeeVm;
    }

    public async Task UpdateAsync(int id, EmployeeUpdateViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new EntityNotFoundException("Neplatný identifikátor zaměstnance.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<EmployeePositionUpdateViewModel> GetForPositionUpdateAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var employeeVm = mapper.Map<EmployeePositionUpdateViewModel>(employee);
        employeeVm.Positions = await GetPositionsSelectListAsync(ct);

        foreach (var position in employeeVm.Positions)
        {
            if (position.Value == employeeVm.PositionId.ToString())
            {
                position.Selected = true;
                break;
            }
        }

        return employeeVm;
    }

    public async Task UpdatePositionAsync(int id, EmployeePositionUpdateViewModel model, CancellationToken ct)
    {
        if (id != model.EmployeeId)
        {
            throw new EntityNotFoundException("Neplatný identifikátor zaměstnance.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var position = await unitOfWork.PositionRepository.GetWithoutDependenciesAsync(model.PositionId, ct);
        if (position is null || !position.IsActive)
        {
            throw new Exception("Nově zvolená pozice nebyla nalezena nebo již není aktivní.");
        }

        mapper.Map(model, employee);

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
