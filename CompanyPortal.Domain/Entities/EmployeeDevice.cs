using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Zařízení zaměstnance")]
public sealed class EmployeeDevice : IHasId
{
    [NotFilterable]
    public int Id { get; set; }

    [NotFilterable]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    [NotFilterable]
    public int DeviceId { get; set; }
    public Device Device { get; set; } = default!;

    [FilterOrder(1)]
    [FilterLabel("Zařízení zapůjčeno")]
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    [FilterOrder(2)]
    [FilterLabel("Zařízení vráceno")]
    public DateTime? ReturnedAt { get; set; }

    [FilterOrder(3)]
    [FilterLabel("Poznámka k zapůjč. zařízení")]
    public string? Note { get; set; }
}