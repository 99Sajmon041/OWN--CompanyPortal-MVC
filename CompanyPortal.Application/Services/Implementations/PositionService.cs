using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Position;
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
            throw new EntityNotFoundException("Pozice nebyla nalezena.");
        }

        var positionVm = mapper.Map<PositionDetailViewModel>(position);

        return positionVm;
    }

    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var position = await unitOfWork.PositionRepository.GetByIdAsync(id, ct);
        if (position is null)
        {
            throw new EntityNotFoundException("Pozice nebyla nalezena.");
        }

        if (position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Status pozice administrator nelze změnit.");
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
        var positionLevels = GetPositionLevels();

        return new PositionUpsertViewModel
        {
            Departments = deparments,
            PositionLevels = positionLevels
        };
    }

    public async Task CreateAsync(PositionUpsertViewModel model, CancellationToken ct)
    {
        var existsSameName = await unitOfWork.PositionRepository.ExistsAnyWithSameParametersAsync(model.PositionName, ct);
        if (existsSameName)
        {
            throw new Exception("Pozice se stejným názvem již existuje.");
        }

        if (model.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Pozici administrátora nelze vytvořit ani upravit.");
        }

        if (model.PositionLevel != PositionLevel.Member)
        {
            var existsManagingPosition = await unitOfWork.PositionRepository.ExistsManagingPositionInDepartmentAsync(model.DepartmentId, ct);

            if (existsManagingPosition)
            {
                throw new Exception("V daném oddělení již existuje vedoucí pozice.");
            }
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
            throw new EntityNotFoundException("Pozice nebyla nalezena.");
        }

        if (position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Pozici administrátora nelze upravit.");
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

        var positionLevels = GetPositionLevels();
        var positionVm = mapper.Map<PositionUpsertViewModel>(position);

        positionVm.Departments = deparments;
        positionVm.PositionLevels = positionLevels;

        return positionVm;
    }

    public async Task UpdateAsync(int id, PositionUpsertViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new EntityNotFoundException("Neplatný identifikátor pozice.");
        }

        var position = await unitOfWork.PositionRepository.GetWithoutDependenciesAsync(id, ct);
        if (position is null)
        {
            throw new EntityNotFoundException("Pozice nebyla nalezena.");
        }

        if (position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Pozici administrátora nelze upravit.");
        }

        if (model.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Pozici administrátora nelze vytvořit ani upravit.");
        }

        if (model.PositionLevel != PositionLevel.Member)
        {
            var existsManagingPosition = await unitOfWork.PositionRepository.ExistsManagingPositionInDepartmentAsync(
                model.DepartmentId,
                position.Id, 
                ct);

            if (existsManagingPosition)
            {
                throw new Exception("V daném oddělení již existuje vedoucí pozice.");
            }
        }

        bool existsWithSameParameters = await unitOfWork.PositionRepository.ExistsAnyWithSameParametersAsync(model.PositionName, model.Id, ct);
        if (existsWithSameParameters)
        {
            throw new Exception("Pozice se stejným názvem již existuje.");
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

    private List<SelectListItem> GetPositionLevels()
    {
        var positionLevels = OptionsBuilder.GetEnumSelectList<PositionLevel>();

        foreach (var positionLevel in positionLevels)
        {
            if (positionLevel.Text == PositionLevel.Administrator.GetDisplayName())
            {
                positionLevel.Disabled = true;
                break;
            }
        }

        return positionLevels;
    }
}
