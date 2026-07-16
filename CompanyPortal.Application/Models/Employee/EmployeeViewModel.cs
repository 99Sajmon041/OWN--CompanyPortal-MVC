using CompanyPortal.Application.Models.EmployeeDevice;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeViewModel
{
        [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Osobní číslo")]
    public string PersonalNumber { get; set; } = default!;

    [Display(Name = "Jméno")]
    public string FirstName { get; set; } = default!;

    [Display(Name = "Příjmení")]
    public string LastName { get; set; } = default!;

    [Display(Name = "Celé jméno")]
    public string FullName => $"{FirstName} {LastName}";

    [Display(Name = "ID uživatele")]
    public string ApplicationUserId { get; set; } = default!;

    [Display(Name = "Název pozice")]
    public string PositionName { get; set; } = default!;

    [Display(Name = "Stav zaměstnance")]
    public bool IsActive { get; set; }

    [Display(Name = "Zaměstnanec vytvořen")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Jméno nadřízeného")]
    public string? SupervisorName { get; set; }

    [Display(Name = "Obrázek zaměstnance")]
    public string ImageUrl { get; set; } = default!;

    [Display(Name = "Role zaměstnance")]
    public ICollection<string> Roles { get; set; } = [];

    [Display(Name = "Zap. zařízení zaměstnance")]
    public ICollection<EmployeeDeviceViewModel> EmployeeDevices { get; set; } = [];
}
