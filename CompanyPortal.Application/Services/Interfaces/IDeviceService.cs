using CompanyPortal.Application.Models.Device;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IDeviceService
{
    Task<PagedResult<DeviceListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<DeviceDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
    DeviceUpsertViewModel GetForUpsertAsync();
    Task CreateAsync(DeviceUpsertViewModel model, CancellationToken ct);
    Task<DeviceUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct);
    Task UpdateAsync(int id, DeviceUpsertViewModel model, CancellationToken ct);
    Task DeprecateAsync(int id, CancellationToken ct);
}