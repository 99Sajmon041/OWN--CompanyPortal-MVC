namespace CompanyPortal.Application.Models.Building;

public sealed class BuildingUpsertViewModel
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
}
