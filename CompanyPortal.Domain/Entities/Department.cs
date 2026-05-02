using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.Entities;

public sealed class Department : IHasId, IHasActive
{
    public int Id { get; set; }
    public string DepartmentName { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<Position> Positions { get; set; } = [];
    public Building Building { get; set; } = default!;
    public int BuildingId { get; set; }
}