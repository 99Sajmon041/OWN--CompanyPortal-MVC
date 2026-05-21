using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Domain.Entities;

public sealed class Device
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public string DeviceName { get; set; } = default!;
    public string? Note { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public ICollection<EmployeeDevice> EmployeeDevices { get; set; } = [];
}