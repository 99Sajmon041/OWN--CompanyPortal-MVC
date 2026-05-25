using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Models.Device;

public sealed class DeviceListItemViewModel
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public string DeviceName { get; set; } = default!;
    public string? Note { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeprecated { get; set; }
}
