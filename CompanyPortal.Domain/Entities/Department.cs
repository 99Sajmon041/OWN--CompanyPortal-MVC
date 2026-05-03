using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Oddělení")]
public sealed class Department : IHasId, IHasActive
{
    [NotFilterable]
    public int Id { get; set; }

    [FilterOrder(1)]
    [FilterLabel("Název oddělení")]
    public string DepartmentName { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Stav oddělení")]
    public bool IsActive { get; set; }

    [NotFilterable]
    public ICollection<Position> Positions { get; set; } = [];

    public Building Building { get; set; } = default!;

    [NotFilterable]
    public int BuildingId { get; set; }
}