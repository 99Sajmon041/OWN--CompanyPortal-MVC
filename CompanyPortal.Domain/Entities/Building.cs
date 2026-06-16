using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Budova")]
public sealed class Building : IHasId, IHasActive
{
    [NotFilterable]
    public int Id { get; set; }

    [FilterOrder(1)]
    [FilterLabel("Název budovy")]
    public string BuildingName { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Popis budovy")]
    public string? Description { get; set; }

    [FilterOrder(3)]
    [FilterLabel("Kód budovy")]
    public string Code { get; set; } = default!;

    [FilterOrder(4)]
    [FilterLabel("Stav budovy")]
    public bool IsActive { get; set; }

    [NotFilterable]
    public ICollection<Department> Departments { get; set; } = [];
}