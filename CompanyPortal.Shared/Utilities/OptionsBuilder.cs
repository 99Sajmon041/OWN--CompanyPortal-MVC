using CompanyPortal.Shared.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Shared.Utilities;

public static class OptionsBuilder
{
    public static List<SelectListItem> GetPageSizeOptions()
    {
        return
        [
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" }
        ];
    }

    public static List<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(x => new SelectListItem
            {
                Value = Convert.ToInt32(x).ToString(),
                Text = x.GetDisplayName()
            })
            .ToList();
    }

    public static List<string> GetAdditionalRoles()
    {
        return
        [
            AppRoles.HRManager,
            AppRoles.ReceptionManager,
            AppRoles.LunchManager,
            AppRoles.AttendanceManager,
            AppRoles.PayrollManager
        ];
    }
}
