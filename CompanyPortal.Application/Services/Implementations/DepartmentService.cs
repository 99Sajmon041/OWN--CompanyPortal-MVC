using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Enums;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class DepartmentService(IUnitOfWork unitOfWork, IMapper mapper) : IDepartmentService
{
    public async Task<PagedResult<DepartmentListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<Department> query = unitOfWork.DepartmentRepository.GetAllQueryableWithBuilding();

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query
            .ProjectTo<DepartmentListItemViewModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return new PagedResult<DepartmentListItemViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(Department)
        };
    }

    public async Task<DepartmentDetailViewModel> GetDetailAsync(int id, CancellationToken ct)
    {
        var department = await unitOfWork.DepartmentRepository.GetByIdAsync(id, ct);
        if (department is null)
        {
            throw new EntityNotFoundException("Oddělení nebylo nalezeno.");
        }

        var deparmentVm = mapper.Map<DepartmentDetailViewModel>(department);
        deparmentVm.SupervisorName = await unitOfWork.EmployeeRepository.GetGroupLeaderNameAsync(id, ct) ?? "Není určen";

        return deparmentVm;
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var department = await unitOfWork.DepartmentRepository.GetByIdAsync(id, ct);
        if (department is null)
        {
            throw new EntityNotFoundException("Oddělení nebylo nalezeno.");
        }

        if (department.DepartmentName == SystemDepartment.ALL.ToString())
        {
            throw new Exception("Systémové oddělení ALL nelze upravit ani deaktivovat.");
        }

        string term;

        if (department.IsActive)
        {
            if (!department.Positions.Any(x => x.IsActive))
            {
                department.IsActive = false;
                term = "Deaktivace";
            }
            else
            {
                throw new Exception("Oddělení nelze deaktivovat, má stále aktivní pozice.");
            }
        }
        else
        {
            department.IsActive = true;
            term = "Aktivace";
        }

        await unitOfWork.SaveChangesAsync(ct);

        return term;
    }


    public async Task<DepartmentUpsertViewModel> GetForCreateAsync(CancellationToken ct)
    {
        var buildings = await GetBuildingSelectListItemsAsync(ct);

        return new DepartmentUpsertViewModel
        {
            Buildings = buildings
        };
    }

    public async Task CreateAsync(DepartmentUpsertViewModel model, CancellationToken ct)
    {
        var existsSameName = await unitOfWork.DepartmentRepository.ExistsAnyWithSameParametersAsync(model.DepartmentName, ct);
        if (existsSameName)
        {
            throw new Exception("Oddělení se stejným názvem již existuje.");
        }

        if (!model.BuildingId.HasValue)
        {
            throw new Exception("Budova musí být vybrána.");
        }

        var buildingExists = await unitOfWork.BuildingRepository.ExistsByIdAsync(model.BuildingId.Value, ct);
        if (!buildingExists)
        {
            throw new Exception("Vybraná budova neexistuje.");
        }

        var department = mapper.Map<Department>(model);
        department.IsActive = true;

        await unitOfWork.DepartmentRepository.AddAsync(department, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<DepartmentUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var department = await unitOfWork.DepartmentRepository.GetByIdAsync(id, ct);
        if (department is null)
        {
            throw new EntityNotFoundException("Oddělení nebylo nalezeno.");
        }

        if (department.DepartmentName == SystemDepartment.ALL.ToString())
        {
            throw new Exception("Systémové oddělení ALL nelze upravit.");
        }

        var buildings = await GetBuildingSelectListItemsAsync(ct);
        foreach (var building in buildings)
        {
            if (building.Value == department.BuildingId.ToString())
            {
                building.Selected = true;
                break;
            }
        }

        var departmentVm = mapper.Map<DepartmentUpsertViewModel>(department);
        departmentVm.Buildings = buildings;

        return departmentVm;
    }

    public async Task UpdateAsync(int id, DepartmentUpsertViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new EntityNotFoundException("Neplatný identifikátor oddělení.");
        }

        var department = await unitOfWork.DepartmentRepository.GetWithoutDependenciesAsync(id, ct);
        if (department is null)
        {
            throw new EntityNotFoundException("Oddělení nebylo nalezeno.");
        }

        if (department.DepartmentName == SystemDepartment.ALL.ToString())
        {
            throw new Exception("Systémové oddělení ALL nelze upravit.");
        }

        if (!model.BuildingId.HasValue)
        {
            throw new Exception("Budova musí být vybrána.");
        }

        if (model.BuildingId.Value != department.BuildingId)
        {
            var buildingExists = await unitOfWork.BuildingRepository.ExistsByIdAsync(model.BuildingId.Value, ct);
            if (!buildingExists)
            {
                throw new Exception("Vybraná budova neexistuje.");
            }
        }

        var existsSameName = await unitOfWork.DepartmentRepository.ExistsAnyWithSameParametersAsync(model.DepartmentName, department.Id, ct);
        if (existsSameName)
        {             
            throw new Exception("Oddělení se stejným názvem již existuje.");
        }

        mapper.Map(model, department);

        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<List<SelectListItem>> GetBuildingSelectListItemsAsync(CancellationToken ct)
    {
        var buildings = await unitOfWork.BuildingRepository.GetAllActiveAsync(ct);

        return buildings.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.BuildingName
        })
        .ToList();
    }
}