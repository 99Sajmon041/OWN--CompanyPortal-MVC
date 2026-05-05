using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
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
        var deparment = await unitOfWork.DepartmentRepository.GetByIdAsync(id, ct);
        if (deparment is null)
        {
            throw new Exception("Oddělení nebylo nalezeno.");
        }

        var deparmentVm = mapper.Map<DepartmentDetailViewModel>(deparment);

        return deparmentVm;
    }
}
