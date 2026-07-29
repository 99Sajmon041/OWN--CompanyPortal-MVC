using CompanyPortal.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Device;

public sealed class DeviceUpsertViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Sériové číslo zařízení je povinné.")]
    [StringLength(20, ErrorMessage = "Sériové číslo může mít maximálně 20 znaků.")]
    public string SerialNumber { get; set; } = default!;

    [Required(ErrorMessage = "Typ zařízení je povinný.")]
    [EnumDataType(typeof(DeviceType), ErrorMessage = "Vyberte platný typ zařízení.")]
    public DeviceType? DeviceType { get; set; }

    public List<SelectListItem> DeviceTypes { get; set; } = [];

    [Required(ErrorMessage = "Název zařízení je povinný.")]
    [StringLength(100, ErrorMessage = "Název zařízení může mít maximálně 100 znaků.")]
    public string DeviceName { get; set; } = default!;

    [StringLength(200, ErrorMessage = "Poznámka může mít maximálně 200 znaků.")]
    public string? Note { get; set; }

    [DataType(DataType.Date)]
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeprecated { get; set; }
}