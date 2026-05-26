using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.Device;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Enums;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using CompanyPortal.Shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class DeviceService(IUnitOfWork unitOfWork, IMapper mapper) : IDeviceService
{
    public async Task<PagedResult<DeviceListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<Device> query = unitOfWork.DeviceRepository.GetAllQueryable();

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query
            .ProjectTo<DeviceListItemViewModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return new PagedResult<DeviceListItemViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(Device)
        };
    }

    public async Task<DeviceDetailViewModel> GetDetailAsync(int id, CancellationToken ct)
    {
        var device = await unitOfWork.DeviceRepository.GetDetailByIdAsync(id, ct);
        if (device is null)
        {
            throw new EntityNotFoundException("Zařízení nebylo nalezeno.");
        }

        var deviceVm = mapper.Map<DeviceDetailViewModel>(device);

        return deviceVm;
    }

    public DeviceUpsertViewModel GetForUpsertAsync()
    {
        var deviceTypes = OptionsBuilder.GetEnumSelectList<DeviceType>();

        return new DeviceUpsertViewModel
        {
            DeviceTypes = deviceTypes
        };
    }

    public async Task CreateAsync(DeviceUpsertViewModel model, CancellationToken ct)
    {
        model.SerialNumber = model.SerialNumber.Trim();

        var existsWithSameSn = await unitOfWork.DeviceRepository.ExistsAnyWithSameSnAsync(model.SerialNumber, ct);
        if (existsWithSameSn)
        {
            throw new Exception("Zařízení se stejným sériovým číslem již existuje.");
        }

        var device = mapper.Map<Device>(model);
        device.IsDeprecated = false;

        await unitOfWork.DeviceRepository.AddAsync(device, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<DeviceUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var device = await unitOfWork.DeviceRepository.GetWithoutDependenciesAsync(id, ct);
        if (device is null)
        {
            throw new EntityNotFoundException("Zařízení nebylo nalezeno.");
        }

        var deviceVm = mapper.Map<DeviceUpsertViewModel>(device);
        deviceVm.DeviceTypes = OptionsBuilder.GetEnumSelectList<DeviceType>();

        foreach (var type in deviceVm.DeviceTypes)
        {
            if (type.Value == device.DeviceType.ToString())
            {
                type.Selected = true;
                break;
            }
        }

        return deviceVm;
    }

    public async Task UpdateAsync(int id, DeviceUpsertViewModel model, CancellationToken ct)
    {
        if (id != model.Id)
        {
            throw new EntityNotFoundException("Neplatný identifikátor zařízení.");
        }

        var device = await unitOfWork.DeviceRepository.GetWithoutDependenciesAsync(id, ct);
        if (device is null)
        {
            throw new EntityNotFoundException("Zařízení nebylo nalezeno.");
        }

        var existsWithSameSn = await unitOfWork.DeviceRepository.ExistsAnyWithSameSnAsync(model.SerialNumber, model.Id, ct);
        if (existsWithSameSn)
        {
            throw new Exception("Zařízení se stejným sériovým číslem již existuje.");
        }

        mapper.Map(model, device);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeprecateAsync(int id, CancellationToken ct)
    {
        var device = await unitOfWork.DeviceRepository.GetWithoutDependenciesAsync(id, ct);
        if (device is null)
        {
            throw new EntityNotFoundException("Zařízení nebylo nalezeno.");
        }

        bool isBorrowed = await unitOfWork.DeviceRepository.HasAnyActiveBorrowingAsync(id, ct);

        if (isBorrowed)
        {
            throw new Exception("Zařízení je zapůjčené, nelze nastavit jako zastaralé.");
        }

        if (device.IsDeprecated)
        {
            throw new Exception("Zařízení je již odepsané.");
        }

        device.IsDeprecated = true;
        await unitOfWork.SaveChangesAsync(ct);
    }
}
