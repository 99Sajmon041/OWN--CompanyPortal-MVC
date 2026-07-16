using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.CurrentIdentity;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Application.Models.EmployeeDevice;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Enums;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using CompanyPortal.Shared.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class EmployeeService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IRoleAssignmentService roleAssignmentService,
    IApplicationUserService applicationUserService,
    ICurrentUserService currentUserService,
    IImageService imageService) : IEmployeeService
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

    public async Task<(EmployeeViewModel?, List<SelectListItem>)> GetDetailAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var employeeVm = mapper.Map<EmployeeViewModel>(employee);

        var roles = await roleAssignmentService.GetUserRolesAsync(employee.ApplicationUserId, ct);
        employeeVm.Roles = roles.ToList();
        employeeVm.SupervisorName = await unitOfWork.EmployeeRepository.GetGroupLeaderNameAsync(employee.Position.DepartmentId, ct)
            ?? "Zatím není určen";

        var employeeDevices = await unitOfWork.EmployeeDeviceRepository.GetBorrowedByEmployeeIdAsync(id, ct);
        employeeVm.EmployeeDevices = mapper.Map<List<EmployeeDeviceViewModel>>(employeeDevices);

        var availableDevices = await GetAllAvailableAsync(ct);

        return (employeeVm, availableDevices);
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        if (employee.Position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("U administrátora není možné měnit status.");
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
        if (model.PositionId is null)
        {
            throw new Exception("Pozice je povinná.");
        }

        var position = await unitOfWork.PositionRepository.GetByIdAsync(model.PositionId.Value, ct);
        if (position is null || !position.IsActive)
        {
            throw new EntityNotFoundException("Zvolená pozice nebyla nalezena nebo není aktivní.");
        }

        if (position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Není možné upravit administrátora.");
        }

        if (position.PositionLevel != PositionLevel.Member)
        {
            bool exists = await unitOfWork.EmployeeRepository.ExistsActiveEmployeeOnPositionAsync(model.PositionId.Value, ct);

            if (exists)
            {
                throw new Exception("Na této vedoucí pozici je již přiřazen aktivní zaměstnanec.");
            }
        }

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
            employee.ImageUrl = await imageService.SaveImageAsync(model.ImageFile, "employees", ct);
            await unitOfWork.EmployeeRepository.AddAsync(employee, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await applicationUserService.MarkEmployeeProfileCompletedAsync(employee.ApplicationUserId, ct);
            await roleAssignmentService.SyncRolesByPositionLevelAsync(employee.ApplicationUserId, position.PositionLevel, ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);

            if (!string.IsNullOrWhiteSpace(employee.ImageUrl))
            {
                imageService.DeleteImage(employee.ImageUrl);
            }

            throw;
        }
    }

    public async Task<EmployeeUpdateViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        if (employee.Position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Není možné upravit administrátora.");
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

        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        if (employee.Position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Není možné upravit administrátora.");
        }

        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            imageService.DeleteImage(employee.ImageUrl);

            employee.ImageUrl = await imageService.SaveImageAsync(model.ImageFile, "employees", ct);
        }

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<EmployeePositionUpdateViewModel> GetForPositionUpdateAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        if (employee.Position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Není možné upravit administrátora.");
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
        if (!model.PositionId.HasValue)
        {
            throw new Exception("Pozice musí být vybrána.");
        }

        if (id != model.EmployeeId)
        {
            throw new EntityNotFoundException("Neplatný identifikátor zaměstnance.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var position = await unitOfWork.PositionRepository.GetByIdAsync(model.PositionId.Value, ct);
        if (position is null || !position.IsActive)
        {
            throw new Exception("Nově zvolená pozice nebyla nalezena nebo již není aktivní.");
        }

        if (position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Pozici administrátora nelze přiřadit.");
        }

        if (position.PositionLevel != PositionLevel.Member)
        {
            bool exists = await unitOfWork.EmployeeRepository.ExistsActiveEmployeeOnPositionAsync(position.Id, employee.Id, ct);

            if (exists)
            {
                throw new Exception("Na této vedoucí pozici je již přiřazen aktivní zaměstnanec.");
            }
        }

        await roleAssignmentService.SyncRolesByPositionLevelAsync(employee.ApplicationUserId, position.PositionLevel, ct);

        mapper.Map(model, employee);

        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<List<SelectListItem>> GetPositionsSelectListAsync(CancellationToken ct)
    {
        var positions = await unitOfWork.PositionRepository.GetAllAsyncWithPositions(ct);

        positions = positions
            .Where(x => x.PositionLevel != PositionLevel.Administrator)
            .ToList();

        return positions.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = $"({x.PositionLevel.GetDisplayName()}) {x.Department.DepartmentName}: {x.PositionName} "
        })
        .ToList();
    }

    public async Task<List<SelectListItem>> GetForUpsertAdditionalRoleAsync(int id, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var additionalRoles = OptionsBuilder.GetAdditionalRoles();

        var userRoles = await roleAssignmentService.GetUserRolesAsync(employee.ApplicationUserId, ct);

        var availableRoles = additionalRoles.Except(userRoles).ToList();

        return availableRoles.Select(role => new SelectListItem
        {
            Value = role,
            Text = role
        })
        .ToList();
    }

    public async Task UpsertAdditionalRoleAsync(int id, string role, CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(id, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var additionalRoles = OptionsBuilder.GetAdditionalRoles();

        if (!additionalRoles.Contains(role))
        {
            throw new Exception("Neplatná role.");
        }

        var userRoles = await roleAssignmentService.GetUserRolesAsync(employee.ApplicationUserId, ct);
        if (userRoles.Any(additionalRoles.Contains))
        {
            throw new Exception("Zaměstnanec již má přiřazenou dodatečnou roli.");
        }

        await roleAssignmentService.AssignRoleAsync(employee.ApplicationUserId, role, ct);
    }

    public async Task<EmployeeProfileViewModel> GetEmployeeProfileAsync(CancellationToken ct)
    {
        var employee = await unitOfWork.EmployeeRepository.GetByApplicationUserIdWithDependenciesAsync(currentUserService.UserId, ct);
        if (employee is null)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen.");
        }

        var employeeVm = mapper.Map<EmployeeProfileViewModel>(employee);

        var roles = await roleAssignmentService.GetUserRolesAsync(employee.ApplicationUserId, ct);
        employeeVm.Roles = roles.ToList();

        if (employee.Position.PositionLevel == PositionLevel.GroupLeader)
        {
            employeeVm.SupervisorName = "Já (vedoucí týmu)";
        }
        else
        {
            employeeVm.SupervisorName = await unitOfWork.EmployeeRepository.GetGroupLeaderNameAsync(employee.Position.DepartmentId, ct)
                ?? "Zatím není určen";
        }

        var subordinates = await unitOfWork.EmployeeRepository.GetAllByDepartmentAsync(employee.Position.DepartmentId, employee.Id, ct);
        if (subordinates.Count > 0)
        {



            employeeVm.SubordinateEmployees = subordinates.Select(x => new EmployeeShortViewModel
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                PositionName = x.Position.PositionName
            })
            .ToList();
        }

        return employeeVm;
    }

    private async Task<List<SelectListItem>> GetAllAvailableAsync(CancellationToken ct)
    {
        var availableDevices = await unitOfWork.DeviceRepository.GetAllAvailableAsync(ct);

        return availableDevices.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = $"{x.DeviceName} - ({x.SerialNumber})"
        })
        .ToList();
    }
}