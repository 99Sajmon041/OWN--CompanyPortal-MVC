using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Department;

public sealed class DepartmentListItemViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název oddělení")]
    public string DepartmentName { get; set; } = default!;

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; }

    [Display(Name = "Název budovy")]
    public string BuildingName { get; set; } = default!;
}