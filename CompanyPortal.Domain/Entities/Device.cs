using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Domain.Entities;

public sealed class Device : IHasId
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public string DeviceName { get; set; } = default!;
    public string? Note { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeprecated { get; set; }
    public ICollection<EmployeeDevice> EmployeeDevices { get; set; } = [];
}