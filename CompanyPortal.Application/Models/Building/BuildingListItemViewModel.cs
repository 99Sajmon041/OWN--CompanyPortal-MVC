using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Building;

public sealed class BuildingListItemViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název budovy")]
    public string BuildingName { get; set; } = default!;

    [Display(Name = "Popis")]
    public string? Description { get; set; }

    [Display(Name = "Kód")]
    public string Code { get; set; } = default!;

    [Display(Name = "Stav")]
    public bool IsActive { get; set; }
}