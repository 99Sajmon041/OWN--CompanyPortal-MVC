using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeePositionUpdateViewModel
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Pozice je povinná")]
    public int PositionId { get; set; }
    public List<SelectListItem> Positions { get; set; } = [];
}
