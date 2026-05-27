using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.EmployeeDevice;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class EmployeeDeviceService(IUnitOfWork unitOfWork, IMapper mapper) : IEmployeeDeviceService
{
    public async Task<PagedResult<EmployeeDeviceViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<EmployeeDevice> query = unitOfWork.EmployeeDeviceRepository.GetAllQueryableWithDependencies();

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query
            .ProjectTo<EmployeeDeviceViewModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return new PagedResult<EmployeeDeviceViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(Device)
        };
    }

    public async Task<List<EmployeeDeviceViewModel>> GetBorrowedByEmployeeIdAsync(int employeeId, CancellationToken ct)
    {
        var borrowedDevices = await unitOfWork.EmployeeDeviceRepository.GetBorrowedByEmployeeIdAsync(employeeId, ct);
        var borrowedDevicesVm = mapper.Map<List<EmployeeDeviceViewModel>>(borrowedDevices);

        return borrowedDevicesVm;
    }
    public async Task AssignDeviceAsync(int employeeId, int deviceId, string? note, CancellationToken ct)
    {
        var device = await unitOfWork.DeviceRepository.GetWithoutDependenciesAsync(deviceId, ct);
        if (device is null)
        {
            throw new EntityNotFoundException("Zařízení nebylo nalezeno.");
        }

        if (device.IsDeprecated)
        {
            throw new Exception("Zařízení nelze zapůjčit, je již zastaralé a vyřazené.");
        }

        bool isDeviceBorrowed = await unitOfWork.DeviceRepository.HasAnyActiveBorrowingAsync(deviceId, ct);
        if (isDeviceBorrowed)
        {
            throw new Exception("Zařízení je již vypůjčeno.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetWithoutDependenciesAsync(employeeId, ct);
        if (employee is null || !employee.IsActive)
        {
            throw new EntityNotFoundException("Zaměstnanec nebyl nalezen nebo není aktivní.");
        }

        var employeeDevice = new EmployeeDevice
        {
            EmployeeId = employeeId,
            DeviceId = deviceId,
            AssignedAt = DateTime.UtcNow,
            Note = note
        };

        await unitOfWork.EmployeeDeviceRepository.AddAsync(employeeDevice, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task MarkAsReturnedAsync(int employeeDeviceId, CancellationToken ct)
    {
        var employeeDevice = await unitOfWork.EmployeeDeviceRepository.GetWithoutDependenciesAsync(employeeDeviceId, ct);
        if (employeeDevice is null)
        {
            throw new EntityNotFoundException("Zapůjčené zařízení nebylo nalezeno.");
        }

        if (employeeDevice.ReturnedAt is not null)
        {
            throw new Exception("Zapůjčené zařízení již bylo vráceno.");
        }

        employeeDevice.ReturnedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);
    }
}
