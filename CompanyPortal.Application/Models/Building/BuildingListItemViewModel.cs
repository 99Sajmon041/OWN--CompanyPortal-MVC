namespace CompanyPortal.Application.Models.Building;

public sealed class BuildingListItemViewModel
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; }
}
