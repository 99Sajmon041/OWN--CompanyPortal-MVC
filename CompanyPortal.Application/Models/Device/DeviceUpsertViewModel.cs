using CompanyPortal.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Device;

public sealed class DeviceUpsertViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string SerialNumber { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public List<SelectListItem> DeviceTypes { get; set; } = [];

    [Required]
    [StringLength(100)]
    public string DeviceName { get; set; } = default!;

    [StringLength(200)]
    public string? Note { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeprecated { get; set; }
}
