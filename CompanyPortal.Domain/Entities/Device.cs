using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Zařízení")]
public sealed class Device : IHasId
{
    [NotFilterable]
    public int Id { get; set; }

    [FilterOrder(1)]
    [FilterLabel("Sériové číslo zařízení")]
    public string SerialNumber { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Typ zařízení")]
    public DeviceType DeviceType { get; set; }

    [FilterOrder(3)]
    [FilterLabel("Název zařízení")]
    public string DeviceName { get; set; } = default!;

    [FilterOrder(4)]
    [FilterLabel("Poznámka k zařízení")]
    public string? Note { get; set; }

    [FilterOrder(5)]
    [FilterLabel("Datum pořízení zařízení")]
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    [FilterOrder(6)]
    [FilterLabel("Zastaralé zařízení")]
    public bool IsDeprecated { get; set; }

    [NotFilterable]
    public ICollection<EmployeeDevice> EmployeeDevices { get; set; } = [];
}