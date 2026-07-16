using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.EmployeeDevice;

public sealed class EmployeeDeviceViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "ID zaměstnance")]
    public int EmployeeId { get; set; }

    [Display(Name = "Zaměstnanec")]
    public string EmployeeFullName { get; set; } = default!;

    [Display(Name = "ID zařízení")]
    public int DeviceId { get; set; }

    [Display(Name = "Zařízení")]
    public string DeviceName { get; set; } = default!;

    [Display(Name = "Zapůjčeno od")]
    public DateTime AssignedAt { get; set; }

    [Display(Name = "Vráceno")]
    public DateTime? ReturnedAt { get; set; }

    [Display(Name = "Poznámka")]
    public string? Note { get; set; }

    [Display(Name = "Sériové číslo")]
    public string SerialNumber { get; set; } = default!;
}