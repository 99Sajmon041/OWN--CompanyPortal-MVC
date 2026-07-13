using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Position;

public sealed class PositionDetailViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Název pozice")]
    public string PositionName { get; set; } = default!;

    [Display(Name = "Úroveň pozice")]
    public PositionLevel PositionLevel { get; set; }

    public int DepartmentId { get; set; }

    [Display(Name = "Oddělení")]
    public string DepartmentName { get; set; } = default!;

    [Display(Name = "Stav pozice")]
    public bool IsActive { get; set; }

    [Display(Name = "Hodinová sazba")]
    public decimal HourlyRate { get; set; }

    public ICollection<EmployeeViewModel> Employees { get; set; } = [];
}
