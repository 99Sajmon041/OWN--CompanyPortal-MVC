using CompanyPortal.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Position;

public sealed class PositionUpsertViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název pozice je povinný")]
    [StringLength(50, ErrorMessage = "Název pozice musí mít 5 - 50 znaků", MinimumLength = 5)]
    public string PositionName { get; set; } = default!;
    public PositionLevel PositionLevel { get; set; }  //readonly (disabled)

    [Range(50, double.MaxValue, ErrorMessage = "Hodinová sazba musí být alespoň 50 Kč.")]
    public decimal HourlyRate { get; set; }
    public ICollection<SelectListItem> Departments { get; set; } = [];

    [Range(1, int.MaxValue, ErrorMessage = "Oddělení je povinné")]
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
}
