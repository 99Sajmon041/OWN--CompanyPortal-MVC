using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Models.Position;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using CompanyPortal.Shared.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class PositionService(IUnitOfWork unitOfWork, IMapper mapper) : IPositionService
{
    public async Task<PagedResult<PositionListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<Position> query = unitOfWork.PositionRepository.GetAllQueryableWithDepartment();

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query
            .ProjectTo<PositionListItemViewModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return new PagedResult<PositionListItemViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(Position)
        };
    }

    public async Task<PositionDetailViewModel> GetDetailAsync(int id, CancellationToken ct)
    {
        var position = await unitOfWork.PositionRepository.GetByIdAsync(id, ct);
        if (position is null)
        {
            throw new Exception("Pozice nebyla nalezena.");
        }

        var positionVm = mapper.Map<PositionDetailViewModel>(position);

        return positionVm;
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var position = await unitOfWork.PositionRepository.GetByIdAsync(id, ct);
        if (position is null)
        {
            throw new Exception("Pozice nebyla nalezena.");
        }

        string term;

        if (position.IsActive)
        {
            if(!position.Employees.Any(x => x.IsActive))
            {
                position.IsActive = false;
                term = "Deaktivace";
            }
            else
            {
                throw new Exception("Pozici nelze deaktivovat, má stále aktivní zaměstnance.");
            }
        }
        else
        {
            position.IsActive = true;
            term = "Aktivace";
        }

        await unitOfWork.SaveChangesAsync(ct);

        return term;
    }

    public async Task<PositionUpsertViewModel> GetForCreateAsync(CancellationToken ct)
    {
        var deparments = await GetDepartmentSelectListItemsAsync(ct);

        return new PositionUpsertViewModel
        {
            Departments = deparments
        };
    }

    public async Task CreateAsync(PositionUpsertViewModel model, CancellationToken ct)
    {
        var existsSameName = await unitOfWork.PositionRepository.ExistsAnyWithSameParametersAsync(model.PositionName, ct);
        if (existsSameName)
        {
            throw new Exception("Pozice se stejným názvem již existuje.");
        }

        var existsManagingPosition = await unitOfWork.PositionRepository.ExistsManagingPositionInDepartmentAsync(model.DepartmentId, ct);
        if (existsManagingPosition)
        {
            throw new Exception($"V daném oddělení již existuje pozice s úrovní: {model.PositionLevel.GetDisplayName()}.");
        }

        var position = mapper.Map<Position>(model);
        position.IsActive = true;

        await unitOfWork.PositionRepository.AddAsync(position, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<PositionUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var position = await unitOfWork.PositionRepository.GetWithoutDependenciesAsync(id, ct);
        if (position is null)
        {
            throw new Exception("Pozice nebyla nalezena.");
        }

        var deparments = await GetDepartmentSelectListItemsAsync(ct);
        foreach (var deparment in deparments)
        {
            if (deparment.Value == position.DepartmentId.ToString())
            {
                deparment.Selected = true;
                break;
            }
        }

        var positionVm = mapper.Map<PositionUpsertViewModel>(position);
        positionVm.Departments = deparments;

        return positionVm;
    }

    public async Task UpdateAsync(int id, PositionUpsertViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new Exception("Neplatný identifikátor pozice.");
        }

        var position = await unitOfWork.PositionRepository.GetWithoutDependenciesAsync(id, ct);
        if (position is null)
        {
            throw new Exception("Pozice nebyla nalezena.");
        }

        bool existsWithSameParameters = await unitOfWork.PositionRepository.ExistsAnyWithSameParametersAsync(model.PositionName, model.Id, ct);
        if (existsWithSameParameters)
        {
            throw new Exception("Pozice se stejným názvem již existuje.");
        }

        if (model.PositionLevel != position.PositionLevel)
        {
            model.PositionLevel = position.PositionLevel;
        }

        mapper.Map(model, position);

        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<List<SelectListItem>> GetDepartmentSelectListItemsAsync(CancellationToken ct)
    {
        var departments = await unitOfWork.DepartmentRepository.GetAllActiveAsync(ct);

        return departments.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.DepartmentName
        })
        .ToList();
    }
}
