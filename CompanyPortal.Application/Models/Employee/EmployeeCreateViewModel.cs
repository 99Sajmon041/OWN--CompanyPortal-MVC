using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeCreateViewModel
{
    [Required(ErrorMessage = "Křestní jméno je povinné.")]
    [StringLength(50, ErrorMessage = "Křestní jméno musí mít 2 - 50 znaků", MinimumLength = 2)]
    public string FirstName { get; set; } = default!;

    [StringLength(50, ErrorMessage = "Příjmení musí mít 2 - 50 znaků", MinimumLength = 2)]
    [Required(ErrorMessage = "Příjmení je povinné.")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Fotka uživatele je povinná")]
    public IFormFile ImageFile { get; set; } = default!;

    [Required(ErrorMessage = "Uživatel je povinný")]
    public string ApplicationUserId { get; set; } = default!;

    [ValidateNever]
    public List<SelectListItem> ApplicationUsers { get; set; } = [];

    [Required(ErrorMessage = "Pozice je povinná.")]
    public int? PositionId { get; set; }

    [ValidateNever]
    public List<SelectListItem> Positions { get; set; } = [];
}
