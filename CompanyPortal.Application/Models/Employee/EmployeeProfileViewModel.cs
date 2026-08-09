using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeProfileViewModel
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

    [Display(Name = "ID aplikačního uživatele")]
    public string ApplicationUserId { get; set; } = default!;

    [Display(Name = "Pozice")]
    public string PositionName { get; set; } = default!;

    [Display(Name = "Oddělení")]
    public string DepartmentName { get; set; } = default!;

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; }

    [Display(Name = "Vytvořeno")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Nadřízený")]
    public string? SupervisorName { get; set; }

    // Property to link Supervisor employee
    public int? SupervisorId { get; set; }

    [Display(Name = "Fotografie")]
    public string ImageUrl { get; set; } = default!;

    [Display(Name = "Podřízení zaměstnanci")]
    public List<EmployeeShortViewModel> SubordinateEmployees { get; set; } = [];

    [Display(Name = "Role")]
    public ICollection<string> Roles { get; set; } = [];
}