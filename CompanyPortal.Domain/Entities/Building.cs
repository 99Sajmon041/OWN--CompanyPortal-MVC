namespace CompanyPortal.Domain.Entities;

public sealed class Building
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public ICollection<Department> Departments { get; set; } = [];
}
