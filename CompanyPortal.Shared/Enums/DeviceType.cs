using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Shared.Enums;

public enum DeviceType
{
    [Display(Name = "Notebook")]
    Notebook = 1,

    [Display(Name = "Mobilní telefon")]
    MobilePhone = 2,

    [Display(Name = "Monitor")]
    Monitor = 3,

    [Display(Name = "Myš")]
    Mouse = 4,

    [Display(Name = "Sluchátka")]
    Headphone = 5
}