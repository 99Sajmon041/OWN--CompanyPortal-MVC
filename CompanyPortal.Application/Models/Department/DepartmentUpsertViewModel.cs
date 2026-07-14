using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Department;

public sealed class DepartmentUpsertViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název oddělení je povinný")]
    [StringLength(50, ErrorMessage = "Název oddělení musí mít 5 - 50 znaků", MinimumLength = 5)]
    public string DepartmentName { get; set; } = default!;
    public bool IsActive { get; set; }

    [ValidateNever]
    public ICollection<SelectListItem> Buildings { get; set; } = [];

    [Required(ErrorMessage = "Vyberte budovu.")]
    public int? BuildingId { get; set; }
}