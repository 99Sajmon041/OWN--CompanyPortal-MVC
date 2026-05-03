using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Pozice")]
public sealed class Position : IHasId, IHasActive
{
    [NotFilterable]
    public int Id { get; set; }

    [FilterOrder(1)]
    [FilterLabel("Název pozice")]
    public string PositionName { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Stupeň pozice")]
    public PositionLevel PositionLevel { get; set; }

    public Department Department { get; set; } = default!;

    [NotFilterable]
    public int DepartmentId { get; set; }

    [FilterOrder(3)]
    [FilterLabel("Stav pozice")]
    public bool IsActive { get; set; }

    [NotFilterable]
    public ICollection<Employee> Employees { get; set; } = [];
}
