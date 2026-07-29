using System.ComponentModel.DataAnnotations;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Models.Device;

public sealed class DeviceListItemViewModel
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Sériové číslo")]
    public string SerialNumber { get; set; } = default!;

    [Display(Name = "Typ zařízení")]
    public DeviceType DeviceType { get; set; }

    [Display(Name = "Název zařízení")]
    public string DeviceName { get; set; } = default!;

    [Display(Name = "Poznámka")]
    public string? Note { get; set; }

    [Display(Name = "Datum pořízení")]
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Vyřazené")]
    public bool IsDeprecated { get; set; }
}